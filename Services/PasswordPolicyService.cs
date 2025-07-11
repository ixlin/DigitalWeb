using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;

namespace DigitalSolutionsWeb.Services
{
    public class PasswordPolicyService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PasswordPolicyService> _logger;

        public PasswordPolicyService(
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PasswordPolicyService> logger)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// 检查当前登录用户是否需要修改密码
        /// </summary>
        public async Task<bool> ShouldUserChangePasswordAsync()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return false;
                }

                // 检查用户是否有PasswordChanged声明，如果没有则认为需要修改密码
                var passwordChanged = await _userManager.GetClaimsAsync(user);
                return !passwordChanged.Any(c => c.Type == "PasswordChanged");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查用户密码状态时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 标记用户已修改过密码
        /// </summary>
        public async Task MarkPasswordAsChangedAsync()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return;
                }

                // 检查用户是否已有PasswordChanged声明
                var claims = await _userManager.GetClaimsAsync(user);
                if (!claims.Any(c => c.Type == "PasswordChanged"))
                {
                    // 添加标记声明
                    await _userManager.AddClaimAsync(user, new Claim("PasswordChanged", DateTime.UtcNow.ToString()));
                    _logger.LogInformation($"用户 {user.Email} 密码已标记为已修改");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "标记密码为已修改时发生错误");
            }
        }

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        private async Task<IdentityUser?> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            return await _userManager.FindByIdAsync(userId);
        }
    }
}
