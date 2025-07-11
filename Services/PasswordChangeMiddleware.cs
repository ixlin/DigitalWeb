using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace DigitalSolutionsWeb.Services
{
    public class PasswordChangeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PasswordChangeMiddleware> _logger;

        public PasswordChangeMiddleware(RequestDelegate next, ILogger<PasswordChangeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, PasswordPolicyService passwordPolicyService)
        {
            // 仅对Admin区域应用密码修改检查
            if (!context.Request.Path.StartsWithSegments("/Admin"))
            {
                await _next(context);
                return;
            }
            
            // 跳过静态资源和特定路径
            if (context.Request.Path.Value?.StartsWith("/Admin/Dashboard/ChangePassword") == true ||
                context.Request.Path.Value?.StartsWith("/css") == true ||
                context.Request.Path.Value?.StartsWith("/js") == true ||
                context.Request.Path.Value?.StartsWith("/lib") == true ||
                context.Request.Path.Value?.StartsWith("/images") == true)
            {
                await _next(context);
                return;
            }
            
            // 如果用户已经认证，检查密码修改需求
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                bool shouldChangePassword = await passwordPolicyService.ShouldUserChangePasswordAsync();
                if (shouldChangePassword)
                {
                    _logger.LogInformation("用户需要修改初始密码");
                    
                    // 重定向到Admin区域的密码修改页面
                    context.Response.Redirect("/Admin/Dashboard/ChangePassword");
                    return;
                }
            }

            await _next(context);
        }
    }

    // 中间件扩展方法
    public static class PasswordChangeMiddlewareExtensions
    {
        public static IApplicationBuilder UsePasswordChangePolicyCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PasswordChangeMiddleware>();
        }
    }
}
