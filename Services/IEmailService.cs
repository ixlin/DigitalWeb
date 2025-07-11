using DigitalSolutionsWeb.Models;

namespace DigitalSolutionsWeb.Services
{
    public interface IEmailService
    {
        Task SendContactNotificationAsync(ContactMessage message);
    }
}
