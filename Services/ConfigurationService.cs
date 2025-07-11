using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DigitalSolutionsWeb.Models;

namespace DigitalSolutionsWeb.Services
{
    public interface IConfigurationService
    {
        Task<SystemSettingsViewModel> GetSystemSettingsAsync();
        Task SaveSystemSettingsAsync(SystemSettingsViewModel settings);
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ConfigurationService> _logger;
        private readonly string _configPath;

        public ConfigurationService(IWebHostEnvironment environment, ILogger<ConfigurationService> logger)
        {
            _environment = environment;
            _logger = logger;
            _configPath = Path.Combine(_environment.ContentRootPath, "appsettings.json");
        }

        public async Task<SystemSettingsViewModel> GetSystemSettingsAsync()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    return new SystemSettingsViewModel();
                }

                var json = await File.ReadAllTextAsync(_configPath);
                var config = JObject.Parse(json);

                var settings = new SystemSettingsViewModel();

                // 读取系统设置
                var systemSettings = config["SystemSettings"];
                if (systemSettings != null)
                {
                    settings.CompanyName = systemSettings["CompanyName"]?.ToString() ?? "Digital Solutions";
                    settings.CompanyAddress = systemSettings["CompanyAddress"]?.ToString() ?? "四川省成都市双流区腾飞二路48号";
                    settings.CompanyPhone = systemSettings["CompanyPhone"]?.ToString() ?? "400-123-4567";
                    settings.CompanyEmail = systemSettings["CompanyEmail"]?.ToString() ?? "contact@digitalsolutions.com";
                }

                // 读取邮件设置
                var emailSettings = config["EmailSettings"];
                if (emailSettings != null)
                {
                    settings.SenderEmail = emailSettings["SenderEmail"]?.ToString() ?? "";
                    settings.SenderName = emailSettings["SenderName"]?.ToString() ?? "";
                    settings.SenderPassword = emailSettings["SenderPassword"]?.ToString() ?? "";
                    settings.AdminEmail = emailSettings["AdminEmail"]?.ToString() ?? "";
                    settings.SmtpServer = emailSettings["SmtpServer"]?.ToString() ?? "";
                    settings.SmtpPort = emailSettings["SmtpPort"]?.ToObject<int>() ?? 465;
                    settings.UseSsl = emailSettings["UseSsl"]?.ToObject<bool>() ?? true;
                }

                return settings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "读取系统设置失败");
                return new SystemSettingsViewModel();
            }
        }

        public async Task SaveSystemSettingsAsync(SystemSettingsViewModel settings)
        {
            try
            {
                var json = await File.ReadAllTextAsync(_configPath);
                var config = JObject.Parse(json);

                // 更新或创建SystemSettings节点
                config["SystemSettings"] = JObject.FromObject(new
                {
                    settings.CompanyName,
                    settings.CompanyAddress,
                    settings.CompanyPhone,
                    settings.CompanyEmail
                });

                // 更新EmailSettings节点
                config["EmailSettings"] = JObject.FromObject(new
                {
                    settings.SenderEmail,
                    settings.SenderName,
                    settings.SenderPassword,
                    settings.AdminEmail,
                    settings.SmtpServer,
                    settings.SmtpPort,
                    settings.UseSsl
                });

                // 格式化JSON并保存
                var formattedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                await File.WriteAllTextAsync(_configPath, formattedJson);

                _logger.LogInformation("系统设置保存成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存系统设置失败");
                throw;
            }
        }
    }
}
