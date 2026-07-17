using HouraDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HouraDAL.Data.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // الـ DbSets لجميع الجداول
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<TimeWallet> TimeWallets => Set<TimeWallet>();
        public DbSet<WalletTransaction> WalletTransactions => Set<WalletTransaction>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ServicePosting> ServicePostings => Set<ServicePosting>();
        public DbSet<PostApplication> PostApplications => Set<PostApplication>();
        public DbSet<ServiceTransaction> ServiceTransactions => Set<ServiceTransaction>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<FiatPackage> FiatPackages => Set<FiatPackage>();
        public DbSet<TimePurchaseInvoice> TimePurchaseInvoices => Set<TimePurchaseInvoice>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // سطر سحري بيقرأ كل الـ Configurations من الـ Assembly الحالية فوراً
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<FiatPackage>().HasData(
        new FiatPackage { PackageId = 1, PackageName = "2 hours", HoursCount = 2, Price = 5.00m },
        new FiatPackage { PackageId = 2, PackageName = "5 hours", HoursCount = 5, Price = 12.00m },
        new FiatPackage { PackageId = 3, PackageName = "10 hours", HoursCount = 10, Price = 20.00m }
    );

            // بيانات مبدئية لأقسام الخدمات (Categories) عشان تظهر في الـ Dropdown بتاع Create Service
            modelBuilder.Entity<Category>().HasData(
        new Category { CategoryId = 1, Name = "Language Learning" },
        new Category { CategoryId = 2, Name = "Handicrafts & Skills" },
        new Category { CategoryId = 3, Name = "Programming & Tech" },
        new Category { CategoryId = 4, Name = "Graphic & UI/UX Design" },
        new Category { CategoryId = 5, Name = "Writing & Translation" },
        new Category { CategoryId = 6, Name = "Academic Tutoring" },
        new Category { CategoryId = 7, Name = "Music & Arts" },
        new Category { CategoryId = 8, Name = "Cooking & Baking" },
        new Category { CategoryId = 9, Name = "Fitness & Sports Coaching" },
        new Category { CategoryId = 10, Name = "Home Repair & Maintenance" },
        new Category { CategoryId = 11, Name = "Business & Career Consulting" },
        new Category { CategoryId = 12, Name = "Pet Care" }
    );
        }
    }
}