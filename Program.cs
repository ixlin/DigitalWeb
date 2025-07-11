using DigitalSolutionsWeb.Data;
using DigitalSolutionsWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(); // 添加Newtonsoft.Json支持

// Add Identity services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    // 暂时关闭邮箱确认要求，方便管理员登录
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure Admin area route and cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    
    // 退出登录后重定向到登录页面
    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    {
        OnSigningOut = context =>
        {
            context.Response.Redirect("/Identity/Account/Login");
            return Task.CompletedTask;
        }
    };
});

// 添加HttpClient服务
builder.Services.AddHttpClient();

// 添加邮件服务
builder.Services.AddScoped<IEmailService, EmailService>();

// 添加配置服务
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

// 添加图片下载服务
builder.Services.AddScoped<IImageDownloadService, SimpleImageDownloadService>();

// 添加HTTP上下文访问器
builder.Services.AddHttpContextAccessor();

// 添加密码策略服务
builder.Services.AddScoped<PasswordPolicyService>();

// 密码修改中间件不需要作为服务注册



var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedRolesAsync(roleManager, logger);
        await SeedAdminUserAsync(userManager, logger);

        // 下载预设图片
        var imageDownloadService = services.GetRequiredService<IImageDownloadService>();
        await imageDownloadService.DownloadPresetImagesAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database or downloading images.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 使用密码策略检查中间件
app.UsePasswordChangePolicyCheck();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger<Program> logger)
{
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
        logger.LogInformation("Role 'Admin' created.");
    }
}

static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager, ILogger<Program> logger)
{
    // 从配置或环境变量获取管理员邮箱，如果未提供则使用默认值
    var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin@digitalsolutions.com";
    
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        // 生成强密码或从环境变量获取
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? GenerateSecurePassword();
        
        var user = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
            
            // 在控制台输出临时凭据信息
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n==================================================");
            Console.WriteLine("       初始管理员账户已创建");
            Console.WriteLine("==================================================");
            Console.WriteLine($"邮箱: {adminEmail}");
            Console.WriteLine($"临时密码: {adminPassword}");
            Console.WriteLine("请首次登录后立即修改此密码!");
            Console.WriteLine("==================================================\n");
            Console.ResetColor();
            
            logger.LogInformation($"Admin user '{adminEmail}' created and assigned to 'Admin' role.");
        }
        else
        {
            logger.LogError($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}

/// <summary>
/// 生成随机强密码
/// </summary>
static string GenerateSecurePassword()
{
    var random = new Random();
    const string upperChars = "ABCDEFGHJKLMNPQRSTUVWXYZ";  // 不包含容易混淆的字符
    const string lowerChars = "abcdefghijkmnopqrstuvwxyz"; // 不包含容易混淆的字符
    const string numberChars = "23456789";                  // 不包含 0/1 避免混淆
    const string specialChars = "!@#$%^&*";
    
    // 确保密码包含各种字符
    var password = new StringBuilder();
    password.Append(upperChars[random.Next(upperChars.Length)]);
    password.Append(lowerChars[random.Next(lowerChars.Length)]);
    password.Append(numberChars[random.Next(numberChars.Length)]);
    password.Append(specialChars[random.Next(specialChars.Length)]);
    
    // 添加额外字符，使总长度达到12位
    const string allChars = upperChars + lowerChars + numberChars + specialChars;
    for (int i = 0; i < 8; i++)
    {
        password.Append(allChars[random.Next(allChars.Length)]);
    }
    
    // 打乱字符顺序
    return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
}
