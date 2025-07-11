using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DigitalSolutionsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactMessagesController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public ContactMessagesController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ContactMessages
        public async Task<IActionResult> Index()
        {
            var messages = await _context.ContactMessages.OrderByDescending(m => m.SubmittedAt).ToListAsync();
            return View(messages);
        }

        // GET: Admin/ContactMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        { 
            if (id == null)
            {
                return NotFound();
            }

            var contactMessage = await _context.ContactMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactMessage == null)
            {
                return NotFound();
            }

            if (!contactMessage.IsRead)
            {
                contactMessage.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(contactMessage);
        }

        // POST: Admin/ContactMessages/MarkAsRead/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.IsRead = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: Admin/ContactMessages/MarkAsUnread/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.IsRead = false;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: Admin/ContactMessages/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
