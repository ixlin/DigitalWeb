using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DigitalSolutionsWeb.Models;
using DigitalSolutionsWeb.Data;
using DigitalSolutionsWeb.Services;
using Microsoft.Extensions.Configuration;

namespace DigitalSolutionsWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public HomeController(
        ILogger<HomeController> logger,
        ApplicationDbContext context,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _emailService = emailService;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        // 首页模型，包含轮播图和服务概览
        var homeViewModel = new HomeViewModel
        {
            HeroSlides = new List<SlideItem>
            {
                new SlideItem { 
                    Title = "数字化转型解决方案", 
                    Description = "帮助企业实现业务流程数字化，提升运营效率", 
                    ImageUrl = "/images/stock/slide1.jpg",
                    ButtonText = "了解更多",
                    ButtonUrl = "/Home/Services"
                },
                new SlideItem { 
                    Title = "智能数据分析平台", 
                    Description = "利用AI技术，从数据中挖掘商业价值", 
                    ImageUrl = "/images/stock/slide2.jpg",
                    ButtonText = "查看案例",
                    ButtonUrl = "/Home/CaseStudies"
                },
                new SlideItem { 
                    Title = "企业级云服务", 
                    Description = "安全、可靠、高效的云基础设施服务", 
                    ImageUrl = "/images/stock/slide3.jpg",
                    ButtonText = "服务详情",
                    ButtonUrl = "/Home/Services"
                }
            },
            
            FeaturedServices = new List<ServiceItem>
            {
                new ServiceItem { 
                    Title = "数字化转型咨询", 
                    Description = "为企业提供全面的数字化转型战略规划和实施路径", 
                    IconClass = "bi bi-graph-up-arrow",
                    Url = "/Home/Services#digital-transformation"
                },
                new ServiceItem { 
                    Title = "企业应用开发", 
                    Description = "定制化企业应用开发，满足特定业务需求", 
                    IconClass = "bi bi-code-square",
                    Url = "/Home/Services#app-development"
                },
                new ServiceItem { 
                    Title = "数据分析与BI", 
                    Description = "数据可视化和商业智能解决方案，助力数据驱动决策", 
                    IconClass = "bi bi-bar-chart",
                    Url = "/Home/Services#data-analytics"
                },
                new ServiceItem { 
                    Title = "云迁移与管理", 
                    Description = "安全高效的云迁移服务和持续的云资源管理", 
                    IconClass = "bi bi-cloud-arrow-up",
                    Url = "/Home/Services#cloud-services"
                }
            },
            
            Statistics = new List<StatItem>
            {
                new StatItem { Value = "200+", Label = "成功案例" },
                new StatItem { Value = "50+", Label = "企业客户" },
                new StatItem { Value = "10年+", Label = "行业经验" },
                new StatItem { Value = "100%", Label = "客户满意度" }
            }
        };
        
        return View(homeViewModel);
    }

    public IActionResult Services()
    {
        var servicesViewModel = new ServicesViewModel
        {
            ServiceCategories = new List<ServiceCategory>
            {
                new ServiceCategory
                {
                    Id = "digital-transformation",
                    Title = "数字化转型咨询",
                    Description = "我们提供全面的数字化转型咨询服务，帮助企业制定数字化战略，优化业务流程，提升运营效率。",
                    ImageUrl = "/images/stock/service-web.jpg",
                    Services = new List<ServiceDetail>
                    {
                        new ServiceDetail { Title = "数字化战略规划", Description = "制定符合企业发展目标的数字化转型战略" },
                        new ServiceDetail { Title = "业务流程优化", Description = "分析并重构业务流程，提高运营效率" },
                        new ServiceDetail { Title = "变革管理", Description = "帮助企业管理数字化转型过程中的组织变革" }
                    }
                },
                new ServiceCategory
                {
                    Id = "app-development",
                    Title = "企业应用开发",
                    Description = "我们提供定制化的企业应用开发服务，包括Web应用、移动应用和企业管理系统等。",
                    ImageUrl = "/images/stock/service-mobile.jpg",
                    Services = new List<ServiceDetail>
                    {
                        new ServiceDetail { Title = "Web应用开发", Description = "响应式、高性能的企业级Web应用" },
                        new ServiceDetail { Title = "移动应用开发", Description = "iOS和Android平台的原生和混合应用开发" },
                        new ServiceDetail { Title = "企业管理系统", Description = "ERP、CRM、OA等企业管理系统的定制开发" }
                    }
                },
                new ServiceCategory
                {
                    Id = "data-analytics",
                    Title = "数据分析与BI",
                    Description = "我们提供专业的数据分析和商业智能解决方案，帮助企业从数据中获取洞察，支持数据驱动决策。",
                    ImageUrl = "/images/stock/service-ai.jpg",
                    Services = new List<ServiceDetail>
                    {
                        new ServiceDetail { Title = "数据仓库建设", Description = "设计和实施企业数据仓库，整合多源数据" },
                        new ServiceDetail { Title = "数据可视化", Description = "直观、交互式的数据可视化仪表板" },
                        new ServiceDetail { Title = "预测分析", Description = "基于机器学习的预测分析模型" }
                    }
                },
                new ServiceCategory
                {
                    Id = "cloud-services",
                    Title = "云迁移与管理",
                    Description = "我们提供安全、高效的云迁移服务和持续的云资源管理，帮助企业充分利用云计算的优势。",
                    ImageUrl = "/images/stock/service-cloud.jpg",
                    Services = new List<ServiceDetail>
                    {
                        new ServiceDetail { Title = "云迁移策略", Description = "制定符合企业需求的云迁移策略和路线图" },
                        new ServiceDetail { Title = "云架构设计", Description = "设计安全、可扩展、高可用的云架构" },
                        new ServiceDetail { Title = "云资源管理", Description = "提供持续的云资源监控、优化和成本管理" }
                    }
                }
            }
        };
        
        return View(servicesViewModel);
    }

    public IActionResult CaseStudies()
    {
        var caseStudiesViewModel = new CaseStudiesViewModel
        {
            CaseStudies = new List<CaseStudy>
            {
                new CaseStudy
                {
                    Title = "某大型制造企业数字化转型",
                    Industry = "制造业",
                    Challenge = "传统制造企业面临生产效率低、信息孤岛、决策滞后等问题，亟需进行数字化转型。",
                    Solution = "我们为客户提供了全面的数字化转型解决方案，包括智能制造系统、企业资源规划系统和数据分析平台。",
                    Results = new List<string>
                    {
                        "生产效率提升30%",
                        "运营成本降低20%",
                        "产品质量缺陷率降低15%",
                        "决策响应时间缩短50%"
                    },
                    ImageUrl = "/images/stock/case-study-1.jpg"
                },
                new CaseStudy
                {
                    Title = "某金融机构智能客服系统",
                    Industry = "金融服务",
                    Challenge = "客户服务中心面临人力成本高、服务质量不稳定、用户体验差等问题。",
                    Solution = "我们开发了基于AI的智能客服系统，整合了自然语言处理、知识图谱和机器学习技术。",
                    Results = new List<string>
                    {
                        "客服人力成本降低40%",
                        "客户问题解决率提升25%",
                        "客户满意度提升30%",
                        "服务响应时间缩短60%"
                    },
                    ImageUrl = "/images/stock/case-study-2.jpg"
                },
                new CaseStudy
                {
                    Title = "某零售连锁企业全渠道数字化",
                    Industry = "零售",
                    Challenge = "传统零售企业面临线上线下渠道割裂、库存管理效率低、用户体验不一致等问题。",
                    Solution = "我们为客户提供了全渠道零售解决方案，包括统一的会员系统、库存管理系统和全渠道营销平台。",
                    Results = new List<string>
                    {
                        "全渠道销售额增长35%",
                        "库存周转率提升25%",
                        "会员复购率提升40%",
                        "营销ROI提升50%"
                    },
                    ImageUrl = "/images/stock/case-study-3.jpg"
                }
            }
        };
        
        return View(caseStudiesViewModel);
    }

    public IActionResult About()
    {
        var aboutViewModel = new AboutViewModel
        {
            CompanyHistory = new List<HistoryItem>
            {
                new HistoryItem { Year = "2015", Title = "公司成立", Description = "公司在上海成立，专注于企业数字化解决方案" },
                new HistoryItem { Year = "2017", Title = "业务扩展", Description = "扩展业务范围，增加数据分析和云服务" },
                new HistoryItem { Year = "2019", Title = "技术创新", Description = "成立AI研究实验室，开发智能数据分析平台" },
                new HistoryItem { Year = "2021", Title = "国际化", Description = "开始拓展国际市场，服务跨国企业客户" },
                new HistoryItem { Year = "2023", Title = "战略升级", Description = "推出新一代数字化转型解决方案框架" }
            },
            
            TeamMembers = new List<TeamMember>
            {
                new TeamMember { Name = "张明", Title = "创始人兼CEO", Bio = "拥有15年IT行业经验，曾在多家知名科技公司担任高管职位", ImageUrl = "/images/stock/team-ceo.jpg" },
                new TeamMember { Name = "李华", Title = "技术总监", Bio = "前Google高级工程师，人工智能和大数据专家", ImageUrl = "/images/stock/team-cto.jpg" },
                new TeamMember { Name = "王芳", Title = "产品总监", Bio = "10年产品管理经验，专注于企业级软件产品设计", ImageUrl = "/images/stock/team-designer.jpg" },
                new TeamMember { Name = "赵强", Title = "解决方案架构师", Bio = "系统架构专家，擅长设计高可用、可扩展的企业级解决方案", ImageUrl = "/images/stock/team-developer.jpg" }
            },
            
            CoreValues = new List<ValueItem>
            {
                new ValueItem { Title = "创新", Description = "不断探索新技术和新方法，为客户创造更大价值", IconClass = "bi bi-lightbulb" },
                new ValueItem { Title = "专业", Description = "专注于自己的专业领域，提供高质量的产品和服务", IconClass = "bi bi-award" },
                new ValueItem { Title = "协作", Description = "与客户和合作伙伴紧密协作，共同实现业务目标", IconClass = "bi bi-people" },
                new ValueItem { Title = "诚信", Description = "诚实守信，建立长期互信的客户关系", IconClass = "bi bi-shield-check" }
            }
        };
        
        return View(aboutViewModel);
    }

    public IActionResult Contact()
    {
        var companyName = _configuration["SystemSettings:CompanyName"] ?? "Digital Solutions";
        var address = _configuration["SystemSettings:CompanyAddress"] ?? "四川省成都市双流区腾飞二路48号";
        var phone = _configuration["SystemSettings:CompanyPhone"] ?? "400-123-4567";
        var email = _configuration["SystemSettings:CompanyEmail"] ?? "contact@digitalsolutions.com";

        var viewModel = new ContactPageViewModel
        {
            CompanyName = companyName,
            Address = address,
            Phone = phone,
            Email = email
        };

        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(ContactPageViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // 创建ContactMessage实体
                var contactMessage = new ContactMessage
                {
                    Name = model.Form.Name,
                    Email = model.Form.Email,
                    Phone = model.Form.Phone,
                    Subject = model.Form.Subject,
                    Message = model.Form.Message,
                    SubmittedAt = DateTime.UtcNow, // 使用更新后的属性
                    IsRead = false // 使用更新后的属性
                };

                // 保存到数据库
                _context.ContactMessages.Add(contactMessage);
                await _context.SaveChangesAsync();

                // 发送邮件通知
                await _emailService.SendContactNotificationAsync(contactMessage);

                _logger.LogInformation($"新留言已保存并发送通知邮件: {contactMessage.Id}");

                // 添加成功消息
                TempData["SuccessMessage"] = "感谢您的留言，我们会尽快与您联系！";
                return RedirectToAction(nameof(Contact));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理留言时发生错误");
                TempData["ErrorMessage"] = "提交留言时发生系统错误，请稍后再试或联系客服。";
                return RedirectToAction(nameof(Contact));
            }
        }
        
        // 如果模型验证失败，重新填充非表单数据并返回视图
        var companyName = _configuration["SystemSettings:CompanyName"] ?? "Digital Solutions";
        var address = _configuration["SystemSettings:CompanyAddress"] ?? "四川省成都市双流区腾飞二路48号";
        var phone = _configuration["SystemSettings:CompanyPhone"] ?? "400-123-4567";
        var email = _configuration["SystemSettings:CompanyEmail"] ?? "contact@digitalsolutions.com";

        model.CompanyName = companyName;
        model.Address = address;
        model.Phone = phone;
        model.Email = email;

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Terms()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
