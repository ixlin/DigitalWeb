using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DigitalSolutionsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Profile()
        {
            ViewData["Title"] = "个人资料";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (user.PhoneNumber != model.PhoneNumber)
            {
                user.PhoneNumber = model.PhoneNumber;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            TempData["SuccessMessage"] = "个人资料已更新";
            return RedirectToAction(nameof(Profile));
        }

        public IActionResult ChangePassword()
        {
            ViewData["Title"] = "更改密码";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "密码已更改";
            return RedirectToAction(nameof(Profile));
        }
    }

    public class ProfileViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        
        [Phone(ErrorMessage = "请输入有效的电话号码")]
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "当前密码是必填项")]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "新密码是必填项")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配")]
        public string ConfirmPassword { get; set; }
    }
}
