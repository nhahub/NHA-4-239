using HouraBLL.Service.Contracts;
using HouraDAL.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HouraPL.Controllers
{
    public class TimeBankController : Controller
    {
        private readonly ITimeBankService _timeBankService;
        private readonly IUnitOfWork _unitOfWork;

        public TimeBankController(ITimeBankService timeBankService, IUnitOfWork unitOfWork)
        {
            _timeBankService = timeBankService;
            _unitOfWork = unitOfWork;
        }

        // صفحة عرض خطط وباقات الشراء purshasehours.html
        public async Task<IActionResult> PurchaseHours()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            // جلب رصيد الساعات للتأكد من حالة التنبيه في الـ Alert العلوي بالصفحة
            var wallet = (await _unitOfWork.Wallets.FindAsync(w => w.UserId == currentUserId)).FirstOrDefault();
            ViewBag.CurrentBalance = wallet?.BalanceInMinutes ?? 0;

            // جلب باقات الـ Seed Data (2h, 5h, 10h) لعرضها في الـ UI
            var packages = await _unitOfWork.FiatPackages.GetAllAsync();
            return View(packages);
        }

        // أكشن معالجة الدفع المالي الحقيقي وشحن دقائق المحفظة
        [HttpPost]
        public async Task<IActionResult> ConfirmPurchase(int packageId, decimal pricePaid)
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            try
            {
                // معالجة البزنس اللوجيك المالي الآمن اللي بيسجل الفاتورة والـ Ledger
                var result = await _timeBankService.ProcessTimePurchaseAsync(currentUserId, packageId, pricePaid);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم الدفع بنجاح! تم شحن الساعات الجديدة في محفظتك الذكية.";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشلت العملية، الباقة المختارة غير صحيحة.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء معالجة الدفع المالي.";
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
