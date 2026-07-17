using HouraBLL.Service.Contracts;
using HouraDAL.Entities;
using HouraDAL.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouraBLL.Service
{
    public class TimeBankService : ITimeBankService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TimeBankService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 1. منطق تبادل الساعات الفعلي بين شخصين بعد إتمام الخدمة
        public async Task<bool> ExecuteTimeExchangeAsync(Guid serviceTransactionId)
        {
            var sTransaction = await _unitOfWork.ServiceTransactions.GetByIdAsync(serviceTransactionId);
            if (sTransaction == null || sTransaction.Status != "Pending") return false;

            var duration = sTransaction.DurationInMinutes;

            // جلب محفظة الطرفين
            var receiverWallet = (await _unitOfWork.Wallets.FindAsync(w => w.UserId == sTransaction.ReceiverId)).FirstOrDefault();
            var providerWallet = (await _unitOfWork.Wallets.FindAsync(w => w.UserId == sTransaction.ProviderId)).FirstOrDefault();

            if (receiverWallet == null || providerWallet == null) return false;

            // الـ Validation الحرج: هل المستفيد عنده ساعات تكفي؟
            if (receiverWallet.BalanceInMinutes < duration)
            {
                throw new InvalidOperationException("عفواً، رصيد المستخدم المستفيد من الخدمة لا يكفي لإتمام هذه العملية. يجب عليه شراء ساعات أولاً.");
            }

            // بدء عملية المعاملة الآمنة بالداتابيز
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // أ) خصم الوقت من الـ Receiver
                receiverWallet.BalanceInMinutes -= duration;
                receiverWallet.LastUpdated = DateTime.UtcNow;
                _unitOfWork.Wallets.Update(receiverWallet);

                // ب) إضافة الوقت للـ Provider
                providerWallet.BalanceInMinutes += duration;
                providerWallet.LastUpdated = DateTime.UtcNow;
                _unitOfWork.Wallets.Update(providerWallet);

                // ج) تسجيل حركة الخصم في الـ Ledger
                var withdrawLog = new WalletTransaction
                {
                    WalletId = receiverWallet.WalletId,
                    Type = "Withdraw",
                    AmountInMinutes = duration,
                    BalanceAfter = receiverWallet.BalanceInMinutes,
                    SourceType = "ServiceTransaction",
                    ServiceTransactionId = sTransaction.TransactionId
                };
                await _unitOfWork.WalletTransactions.AddAsync(withdrawLog);

                // د) تسجيل حركة الإيداع في الـ Ledger
                var depositLog = new WalletTransaction
                {
                    WalletId = providerWallet.WalletId,
                    Type = "Deposit",
                    AmountInMinutes = duration,
                    BalanceAfter = providerWallet.BalanceInMinutes,
                    SourceType = "ServiceTransaction",
                    ServiceTransactionId = sTransaction.TransactionId
                };
                await _unitOfWork.WalletTransactions.AddAsync(depositLog);

                // هـ) تحديث حالة الخدمة لتصبح مكتملة
                sTransaction.Status = "Completed";
                _unitOfWork.ServiceTransactions.Update(sTransaction);

                // حفظ كل شيء وتأكيد الـ Transaction
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception)
            {
                // لو حصلت أي مشكلة في السيرفر الغي كل حاجة فوراً منعاً لعجز الساعات
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        // 2. منطق خيار شراء الساعات بالمال الحقيقي في حالة عدم وجود رصيد
        public async Task<bool> ProcessTimePurchaseAsync(Guid userId, int packageId, decimal amountPaid)
        {
            var package = await _unitOfWork.FiatPackages.GetByIdIntAsync(packageId);
            if (package == null) return false;

            var userWallet = (await _unitOfWork.Wallets.FindAsync(w => w.UserId == userId)).FirstOrDefault();
            if (userWallet == null) return false;

            var minutesToCredit = package.HoursCount * 60; // تحويل الساعات المشتراة لدقائق داخل الداتابيز

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // أ) إنشاء فاتورة الشراء وتوثيقها كـ Success
                var invoice = new TimePurchaseInvoice
                {
                    UserId = userId,
                    PackageId = packageId,
                    AmountPaid = amountPaid,
                    HoursCredited = package.HoursCount,
                    PaymentStatus = "Success",
                    PurchaseDate = DateTime.UtcNow
                };
                await _unitOfWork.Invoices.AddAsync(invoice);
                await _unitOfWork.CompleteAsync(); // لحفظ الفاتورة وتوليد الـ InvoiceId

                // ب) إضافة الدقائق لمحفظة المستخدم
                userWallet.BalanceInMinutes += minutesToCredit;
                userWallet.LastUpdated = DateTime.UtcNow;
                _unitOfWork.Wallets.Update(userWallet);

                // ج) تسجيل الحركة في دفتر الـ Ledger
                var purchaseLog = new WalletTransaction
                {
                    WalletId = userWallet.WalletId,
                    Type = "Deposit",
                    AmountInMinutes = minutesToCredit,
                    BalanceAfter = userWallet.BalanceInMinutes,
                    SourceType = "FiatPurchase",
                    InvoiceId = invoice.InvoiceId
                };
                await _unitOfWork.WalletTransactions.AddAsync(purchaseLog);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}