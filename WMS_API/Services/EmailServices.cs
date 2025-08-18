using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace WEB_API.Services
{
    public class EmailServices
    {
        private readonly string _smtpHost = "mail2.lccgroup.com";  // your SMTP server
        private readonly int _smtpPort = 587;                    // SMTP port (usually 587 for TLS)
        private readonly string _smtpUser = "itis6.ta@lccgroup.com";
        private readonly string _smtpPass = "060925#ic";
        private readonly string _fromEmail = "itis8.ta@lccgroup.com";
        private readonly string _frontendUrl = "https://localhost:7017/reset-password";

        public async Task SendPasswordResetEmail(string toEmail, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("My App (Test)", _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Reset your password";

            // Build reset link with token as query param
            var resetLink = $"{_frontendUrl}?token={token}";

            message.Body = new TextPart("html")
            {
                Text = $"<p>You requested a password reset. Click the link below to reset your password:</p>" +
                       $"<p><a href='{resetLink}'>Reset Password</a></p>" +
                       $"<p>If you didn't request this, please ignore this email.</p>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
