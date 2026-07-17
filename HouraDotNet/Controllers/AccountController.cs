using HouraDAL.Entities;
using HouraDAL.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // عرض صفحة تسجيل الدخول
    [HttpGet]
    public IActionResult Login() => View();

    // استقبال بيانات Login
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = (await _unitOfWork.Users.FindAsync(u => u.Email == email && u.PasswordHash == password)) // فرضية تشفير مبسطة للـ Demo
            .FirstOrDefault();

        if (user == null)
        {
            ModelState.AddModelError("", "خطأ في البريد الإلكتروني أو كلمة المرور.");
            return View();
        }

        // تخزين الـ UserId مؤقتاً في السيشين لإدارة الجلسة
        HttpContext.Session.SetString("UserId", user.UserId.ToString());
        return RedirectToAction("Index", "Dashboard");
    }

    // عرض صفحة التسجيل
    [HttpGet]
    public IActionResult SignUp() => View();

    // استقبال بيانات SignUp من فورم الفرونت إند
    [HttpPost]
    public async Task<IActionResult> SignUp(string Name, string email, string password, string Confirm_Password)
    {
        if (password != Confirm_Password)
        {
            ModelState.AddModelError("", "كلمات المرور غير متطابقة.");
            return View();
        }

        var existingUser = (await _unitOfWork.Users.FindAsync(u => u.Email == email)).FirstOrDefault();
        if (existingUser != null)
        {
            ModelState.AddModelError("", "هذا البريد الإلكتروني مسجل بالفعل.");
            return View();
        }

        // إنشاء المستخدم الجديد
        var newUser = new User
        {
            FullName = Name,
            Email = email,
            PasswordHash = password, // يفضل التشفير بـ BCrypt أو Identity لاحقاً
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            Bio = "Share a little of your time, and gain a lot in return.", // Bio افتراضي مطابق للـ UI
            Skills = "Design,Programming" // مهارات افتراضية قابلة للتعديل
        };

        await _unitOfWork.Users.AddAsync(newUser);
        await _unitOfWork.CompleteAsync();

        // 🌟 إنشاء محفظة الوقت تلقائياً فور التسجيل لمنحه رصيد مبدئي (مثلاً 120 دقيقة = ساعتين ترحيبية)
        var newWallet = new TimeWallet
        {
            UserId = newUser.UserId,
            BalanceInMinutes = 120,
            LastUpdated = DateTime.UtcNow
        };
        await _unitOfWork.Wallets.AddAsync(newWallet);
        await _unitOfWork.CompleteAsync();

        return RedirectToAction(nameof(Login));
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}