using HouraBLL.Service;
using HouraBLL.Service.Contracts;
using HouraDAL.Data.DbContexts;
using HouraDAL.Repository;
using HouraDAL.Repository.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace HouraDotNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. تسجيل الـ MVC والـ Controllers والـ Views للفرونت إند
            builder.Services.AddControllersWithViews();

            // 2. تسجيل اتصال الداتابيز وقراءة الـ Connection String
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 3. تسجيل الـ Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 4. تسجيل خدمات الـ Business Logic لنظام بنك الوقت
            builder.Services.AddScoped<ITimeBankService, TimeBankService>();
            builder.Services.AddScoped<IServicePostingService, ServicePostingService>();
            builder.Services.AddScoped<IPostApplicationService, PostApplicationService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            // 5. إضافة وتجهيز خدمات الـ Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;   // ✅ HttpOnly بيتحط على Cookie مش على options مباشرة
                options.Cookie.IsEssential = true; // ✅ نفس الكلام لـ IsEssential
            });

            var app = builder.Build();

            // Middleware Pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // لازم بعد UseRouting وقبل UseAuthorization
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}