using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ECommerce_Tawj.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;
        public EmailService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_settings.Email),
                Subject = subject,
            };
      

            email.From.Add(new MailboxAddress(_settings.DisplayName, _settings.Email));

            email.To.Add(MailboxAddress.Parse(to));


            email.Body = new BodyBuilder
            {
                HtmlBody = body
            }.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
               _settings.Host,
               _settings.Port,
               SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_settings.Email, _settings.Password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
       
    }
}
