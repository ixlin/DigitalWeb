using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalSolutionsWeb.Data;
using DigitalSolutionsWeb.Models;

namespace DigitalSolutionsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "控制台";
            
            // 获取统计数据
            var totalMessages = await _context.ContactMessages.CountAsync();
            var unreadMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead);
            var todayMessages = await _context.ContactMessages
                .CountAsync(m => m.SubmittedAt.Date == DateTime.Today);

            ViewBag.TotalMessages = totalMessages;
            ViewBag.UnreadMessages = unreadMessages;
            ViewBag.TodayMessages = todayMessages;

            return View();
        }
    }
}
