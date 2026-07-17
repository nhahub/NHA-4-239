using HouraBLL.Service.Contracts;
using HouraDAL.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HouraPL.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServicePostingService _postingService;
        private readonly IPostApplicationService _applicationService;
        private readonly IUnitOfWork _unitOfWork;

        public ServicesController(IServicePostingService postingService, IPostApplicationService applicationService, IUnitOfWork unitOfWork)
        {
            _postingService = postingService;
            _applicationService = applicationService;
            _unitOfWork = unitOfWork;
        }

        // صفحة تصفح الخدمات browse.html مع دعم مربع البحث الخاص بالـ UI
        public async Task<IActionResult> Browse(string searchTerm)
        {
            // استدعاء ميثود جلب البيانات المحسنة التي تدعم الـ SearchTerm
            var postings = await _postingService.GetActivePostingsAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(postings);
        }

        // صفحة خدماتي الشخصية myservices.html
        public async Task<IActionResult> MyServices()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            // جلب الخدمات الخاصة بالمستخدم الحالي فقط
            var myPostings = await _postingService.GetPostingsByUserAsync(currentUserId);
            return View(myPostings);
        }

        // بيفتح فورم إضافة خدمة جديدة (لما نضغط على زرار Create Service)
        [HttpGet]
        public async Task<IActionResult> CreateService()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            ViewBag.Categories = await _unitOfWork.Categories.GetAllAsync();
            return View();
        }

        // بيستقبل بيانات الفورم ويحفظ الخدمة الجديدة في الداتابيز
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(int categoryId, string postType, string title, string description, int estimatedDurationInMinutes)
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            {
                ModelState.AddModelError(string.Empty, "الرجاء ملء كل الحقول المطلوبة.");
                ViewBag.Categories = await _unitOfWork.Categories.GetAllAsync();
                return View();
            }

            try
            {
                await _postingService.CreatePostAsync(currentUserId, categoryId, postType, title, description, estimatedDurationInMinutes);
                TempData["SuccessMessage"] = "تم إنشاء الخدمة بنجاح!";
                return RedirectToAction(nameof(MyServices));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Categories = await _unitOfWork.Categories.GetAllAsync();
                return View();
            }
        }

        // الأكشن الخاص بضغط زرار "Join" في كروت الفرونت إند لتقديم طلب تبادل
        [HttpPost]
        public async Task<IActionResult> JoinService(Guid postId)
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out Guid currentUserId))
                return RedirectToAction("Login", "Account");

            try
            {
                await _applicationService.ApplyForPostAsync(postId, currentUserId);
                TempData["SuccessMessage"] = "تم إرسال طلب الانضمام والتبادل بنجاح للطرف الآخر!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Browse));
        }
    }
}