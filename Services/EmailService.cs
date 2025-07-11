using DigitalSolutionsWeb.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace DigitalSolutionsWeb.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendContactNotificationAsync(ContactMessage message)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                var senderEmail = emailSettings["SenderEmail"];
                var senderName = emailSettings["SenderName"];
                var senderPassword = emailSettings["SenderPassword"];
                var adminEmail = emailSettings["AdminEmail"];
                var smtpServer = emailSettings["SmtpServer"];
                var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "465");
                var useSsl = bool.Parse(emailSettings["UseSsl"] ?? "true");

                var email = new MimeMessage();
                
                // 重要：正确设置发件人，确保显示名称
                email.From.Add(new MailboxAddress(senderName, senderEmail));
                email.To.Add(new MailboxAddress("管理员", adminEmail));
                email.Subject = $"新留言通知: {message.Subject}";

                // 添加额外的邮件头以确保发件人名称正确显示
                email.Headers.Add("X-Sender", senderName);
                email.Headers.Add("X-Mailer", "Digital Solutions Website");

                var builder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                            <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; border-radius: 10px 10px 0 0;'>
                                <h2 style='margin: 0;'><i class='fas fa-envelope'></i> 新的联系消息</h2>
                            </div>
                            <div style='background: #f8f9fa; padding: 20px; border-radius: 0 0 10px 10px;'>
                                <table style='width: 100%; border-collapse: collapse;'>
                                    <tr>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6; font-weight: bold; width: 120px;'>发件人：</td>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6;'>{message.Name}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6; font-weight: bold;'>邮箱：</td>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6;'>{message.Email}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6; font-weight: bold;'>电话：</td>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6;'>{message.Phone ?? "未提供"}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6; font-weight: bold;'>主题：</td>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6;'>{message.Subject}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6; font-weight: bold;'>提交时间：</td>
                                        <td style='padding: 10px; border-bottom: 1px solid #dee2e6;'>{message.SubmittedAt:yyyy-MM-dd HH:mm:ss}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px; font-weight: bold; vertical-align: top;'>消息内容：</td>
                                        <td style='padding: 10px;'><div style='background: white; padding: 15px; border-radius: 5px; border-left: 4px solid #667eea;'>{message.Message.Replace("\n", "<br>")}</div></td>
                                    </tr>
                                </table>
                            </div>
                            <div style='text-align: center; padding: 20px; color: #6c757d; font-size: 12px;'>
                                此邮件由系统自动发送，请勿直接回复
                            </div>
                        </div>
                    "
                };

                email.Body = builder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, smtpPort, useSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"已成功发送留言通知邮件至 {adminEmail}，发件人显示为: {senderName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送留言通知邮件时发生错误");
                throw;
            }
        }
    }
}
