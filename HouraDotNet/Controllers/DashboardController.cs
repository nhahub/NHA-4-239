using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using HouraDAL.Repository.Contracts;

namespace HouraPL.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // تجميع بيانات dashboard.html
        public async Task<IActionResult> Index()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            // جلب رصيد محفظة الوقت
            var wallet = (await _unitOfWork.Wallets.FindAsync(w => w.UserId == currentUserId)).FirstOrDefault();
            int balanceMinutes = wallet?.BalanceInMinutes ?? 0;
            int hours = balanceMinutes / 60;
            int minutes = balanceMinutes % 60;

            // حساب الإحصائيات المطلوبة في الـ UI
            var activeServicesCount = (await _unitOfWork.ServicePostings.FindAsync(p => p.UserId == currentUserId && p.IsAvailable == true)).Count();
            var ongoingRequestsCount = (await _unitOfWork.ServiceTransactions.FindAsync(t => (t.ProviderId == currentUserId || t.ReceiverId == currentUserId) && t.Status == "Pending")).Count();

            // تمرير البيانات للـ View (يمكن استخدام ViewBag لتركيبها على الـ IDs المحددة في الـ HTML)
            ViewBag.ActiveServices = activeServicesCount;
            ViewBag.TimeBalance = $"{hours}h {minutes}m";
            ViewBag.OngoingRequests = ongoingRequestsCount;

            return View();
        }

        // تجميع بيانات صفحة البروفايل myprofile.html
        public async Task<IActionResult> Profile()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            var user = await _unitOfWork.Users.GetByIdAsync(currentUserId);
            if (user == null) return NotFound();

            // جلب التقييمات والمراجعات المكتوبة عن المستخدم
            var reviews = await _unitOfWork.Reviews.FindAsync(r => r.RevieweeUserId == currentUserId);
            ViewBag.Reviews = reviews;
            ViewBag.AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 5.0;

            return View(user);
        }

        // بيفتح فورم تعديل بيانات البروفايل (لما نضغط على زرار Edit Profile)
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            var user = await _unitOfWork.Users.GetByIdAsync(currentUserId);
            if (user == null) return NotFound();

            return View(user);
        }

        // بيستقبل التعديلات ويحفظها في الداتابيز
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(string fullName, string? phoneNumber, string? bio, string? skills)
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            var user = await _unitOfWork.Users.GetByIdAsync(currentUserId);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ModelState.AddModelError(string.Empty, "الاسم مطلوب.");
                return View(user);
            }

            user.FullName = fullName;
            user.PhoneNumber = phoneNumber;
            user.Bio = bio;
            user.Skills = skills;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Profile));
        }

        // تجميع بيانات صفحة الأنشطة/الطلبات Activity.html
        public async Task<IActionResult> Activity()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            // كل المعاملات اللي المستخدم طرف فيها (سواء مقدم الخدمة أو مستقبلها)
            var transactions = (await _unitOfWork.ServiceTransactions.FindAsync(
                t => t.ProviderId == currentUserId || t.ReceiverId == currentUserId))
                .OrderByDescending(t => t.TransactionDate);

            return View(transactions);
        }

        // تجميع بيانات صفحة الإشعارات notification.html
        public async Task<IActionResult> Notifications()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            // جلب الإشعارات الخاصة باليوزر مرتبة من الأحدث للأقدم
            var notifications = (await _unitOfWork.Notifications.FindAsync(n => n.UserId == currentUserId))
                                .OrderByDescending(n => n.CreatedAt);

            return View(notifications);
        }
    }
}