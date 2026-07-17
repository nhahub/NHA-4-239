using HouraDAL.Entities;
using System;
using System;
using System.Threading.Tasks;
using HouraDAL.Entities; // تأكدي إن الـ Namespace ده بيشاور على فولدر الـ Entities الصح عندك

namespace HouraDAL.Repository.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<TimeWallet> Wallets { get; }
        IGenericRepository<WalletTransaction> WalletTransactions { get; }
        IGenericRepository<ServiceTransaction> ServiceTransactions { get; }
        IGenericRepository<TimePurchaseInvoice> Invoices { get; }
        IGenericRepository<FiatPackage> FiatPackages { get; }

        IGenericRepository<PostApplication> PostApplications { get; }
        IGenericRepository<ServicePosting> ServicePostings { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<Notification> Notifications { get; }
        IGenericRepository<Category> Categories { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}