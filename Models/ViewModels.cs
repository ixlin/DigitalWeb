using System.ComponentModel.DataAnnotations;

namespace DigitalSolutionsWeb.Models
{
    // 首页视图模型
    public class HomeViewModel
    {
        public List<SlideItem> HeroSlides { get; set; } = new List<SlideItem>();
        public List<ServiceItem> FeaturedServices { get; set; } = new List<ServiceItem>();
        public List<StatItem> Statistics { get; set; } = new List<StatItem>();
    }

    // 轮播图项
    public class SlideItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public string ButtonUrl { get; set; } = string.Empty;
    }

    // 服务项
    public class ServiceItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    // 统计项
    public class StatItem
    {
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    // 服务页面视图模型
    public class ServicesViewModel
    {
        public List<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();
    }

    // 服务类别
    public class ServiceCategory
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<ServiceDetail> Services { get; set; } = new List<ServiceDetail>();
    }

    // 服务详情
    public class ServiceDetail
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    // 案例研究页面视图模型
    public class CaseStudiesViewModel
    {
        public List<CaseStudy> CaseStudies { get; set; } = new List<CaseStudy>();
    }

    // 案例研究
    public class CaseStudy
    {
        public string Title { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Challenge { get; set; } = string.Empty;
        public string Solution { get; set; } = string.Empty;
        public List<string> Results { get; set; } = new List<string>();
        public string ImageUrl { get; set; } = string.Empty;
    }

    // 关于我们页面视图模型
    public class AboutViewModel
    {
        public List<HistoryItem> CompanyHistory { get; set; } = new List<HistoryItem>();
        public List<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public List<ValueItem> CoreValues { get; set; } = new List<ValueItem>();
    }

    // 历史项
    public class HistoryItem
    {
        public string Year { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    // 团队成员
    public class TeamMember
    {
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }

    // 价值观项
    public class ValueItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
    }

    // 联系我们表单视图模型
    public class ContactViewModel
    {
        [Required(ErrorMessage = "请输入您的姓名")]
        [Display(Name = "姓名")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "请输入您的电子邮箱")]
        [EmailAddress(ErrorMessage = "请输入有效的电子邮箱地址")]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "请输入您的电话号码")]
        [Phone(ErrorMessage = "请输入有效的电话号码")]
        [Display(Name = "电话")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "请选择咨询主题")]
        [Display(Name = "咨询主题")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "请输入您的留言内容")]
        [Display(Name = "留言内容")]
        [StringLength(1000, ErrorMessage = "留言内容不能超过1000个字符")]
        public string Message { get; set; } = string.Empty;
    }

    public class AdminProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        
        [Display(Name = "用户名")]
        public string UserName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "邮箱是必填项")]
        [EmailAddress(ErrorMessage = "请输入有效的邮箱地址")]
        [Display(Name = "邮箱")]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "手机号")]
        [Phone(ErrorMessage = "请输入有效的手机号")]
        public string? PhoneNumber { get; set; }
    }

    public class SystemSettingsViewModel
    {
        [Required(ErrorMessage = "公司名称是必填项")]
        [Display(Name = "公司名称")]
        public string CompanyName { get; set; } = "Digital Solutions";

        [Required(ErrorMessage = "公司地址是必填项")]
        [Display(Name = "公司地址")]
        public string CompanyAddress { get; set; } = "四川省成都市双流区腾飞二路48号";

        [Required(ErrorMessage = "联系电话是必填项")]
        [Display(Name = "联系电话")]
        public string CompanyPhone { get; set; } = "400-123-4567";

        [Required(ErrorMessage = "联系邮箱是必填项")]
        [EmailAddress(ErrorMessage = "请输入有效的邮箱地址")]
        [Display(Name = "联系邮箱")]
        public string CompanyEmail { get; set; } = "contact@digitalsolutions.com";

        // 邮件设置部分
        [Required(ErrorMessage = "发送邮箱是必填项")]
        [EmailAddress(ErrorMessage = "请输入有效的邮箱地址")]
        [Display(Name = "发送邮箱")]
        public string SenderEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "发送者名称是必填项")]
        [Display(Name = "发送者名称")]
        public string SenderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "邮箱密码是必填项")]
        [Display(Name = "邮箱密码")]
        public string SenderPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "管理员邮箱是必填项")]
        [EmailAddress(ErrorMessage = "请输入有效的邮箱地址")]
        [Display(Name = "管理员邮箱")]
        public string AdminEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "SMTP服务器是必填项")]
        [Display(Name = "SMTP服务器")]
        public string SmtpServer { get; set; } = string.Empty;

        [Required(ErrorMessage = "SMTP端口是必填项")]
        [Display(Name = "SMTP端口")]
        [Range(1, 65535, ErrorMessage = "端口范围应在1到65535之间")]
        public int SmtpPort { get; set; } = 465;

        [Display(Name = "启用SSL")]
        public bool UseSsl { get; set; } = true;
    }

    public class ContactPageViewModel
    {
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public ContactViewModel Form { get; set; } = new ContactViewModel();
    }
}
