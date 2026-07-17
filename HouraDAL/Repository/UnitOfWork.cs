using HouraDAL.Data.DbContexts;
using HouraDAL.Entities;
using HouraDAL.Repository.Contracts;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace HouraDAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            // تهيئة الـ Repositories في الـ Constructor
            Users = new GenericRepository<User>(_context);
            Wallets = new GenericRepository<TimeWallet>(_context);
            WalletTransactions = new GenericRepository<WalletTransaction>(_context);
            ServiceTransactions = new GenericRepository<ServiceTransaction>(_context);
            Invoices = new GenericRepository<TimePurchaseInvoice>(_context);
            FiatPackages = new GenericRepository<FiatPackage>(_context);
            PostApplications = new GenericRepository<PostApplication>(_context);
            ServicePostings = new GenericRepository<ServicePosting>(_context);
            Reviews = new GenericRepository<Review>(_context);
            Notifications = new GenericRepository<Notification>(_context);
            Categories = new GenericRepository<Category>(_context);
        }

        // الـ Properties الخاصة بالـ Repositories لربط الـ Interface بالكامل
        public IGenericRepository<User> Users { get; private set; }
        public IGenericRepository<TimeWallet> Wallets { get; private set; }
        public IGenericRepository<WalletTransaction> WalletTransactions { get; private set; }
        public IGenericRepository<ServiceTransaction> ServiceTransactions { get; private set; }
        public IGenericRepository<TimePurchaseInvoice> Invoices { get; private set; }
        public IGenericRepository<FiatPackage> FiatPackages { get; private set; }
        public IGenericRepository<PostApplication> PostApplications { get; private set; }
        public IGenericRepository<ServicePosting> ServicePostings { get; private set; }
        public IGenericRepository<Review> Reviews { get; private set; }
        public IGenericRepository<Notification> Notifications { get; private set; }
        public IGenericRepository<Category> Categories { get; private set; }
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this); 
        }
    }
}