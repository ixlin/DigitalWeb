using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DigitalSolutionsWeb.Services;

namespace DigitalSolutionsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly PasswordPolicyService _passwordPolicyService;

        public DashboardController(
            ILogger<DashboardController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            PasswordPolicyService passwordPolicyService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordPolicyService = passwordPolicyService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("访问管理后台控制面板");
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("管理员已退出登录");
            return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = "/Admin" });
        }
    }
}
