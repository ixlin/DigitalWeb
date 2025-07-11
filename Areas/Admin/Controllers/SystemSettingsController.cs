using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DigitalSolutionsWeb.Services;
using DigitalSolutionsWeb.Models;

namespace DigitalSolutionsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SystemSettingsController : Controller
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<SystemSettingsController> _logger;

        public SystemSettingsController(IConfigurationService configurationService, ILogger<SystemSettingsController> logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var settings = await _configurationService.GetSystemSettingsAsync();
                return View(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取系统设置失败");
                TempData["ErrorMessage"] = "获取系统设置失败，请稍后重试。";
                return View(new SystemSettingsViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SystemSettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _configurationService.SaveSystemSettingsAsync(model);
                TempData["SuccessMessage"] = "系统设置保存成功！配置已更新。";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存系统设置失败");
                ModelState.AddModelError("", "保存失败，请稍后重试。");
                return View(model);
            }
        }
    }
}
