using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration, IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException(nameof(to), "Recipient email address cannot be null or empty.");

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(to));

            using (var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = _smtpSettings.EnableSsl;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }

    }
}