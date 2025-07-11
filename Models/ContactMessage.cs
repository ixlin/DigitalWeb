using System.ComponentModel.DataAnnotations;

namespace DigitalSolutionsWeb.Models
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "姓名是必填项")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "邮箱是必填项")]
        [EmailAddress(ErrorMessage = "请输入有效的邮箱地址")]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [Required(ErrorMessage = "主题是必填项")]
        [StringLength(100)]
        public string Subject { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "消息内容是必填项")]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;
        
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsRead { get; set; } = false;
    }
}
