using LeadXTechnologiesApi.DTOs;
using LeadXTechnologiesApi.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace LeadXTechnologiesApi.Services
{
    public class EmailService : IEmailService
    {
        public readonly EmailSettings _settings;
        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;       
        }
        public async Task SendContactEmailAsync(ContactRequestDto request)
        {
            var email = new MimeMessage();
            email.From.Add(
                new MailboxAddress(
                    _settings.SenderName,
                    _settings.SenderEmail
                ));
            email.To.Add(
                MailboxAddress.Parse(_settings.ReceiverEmail));

            email.Subject = "New LeadX Technologies Contact Inquiry";

            email.Body = new TextPart("html")
            {
                Text = $@"
                    <h2>New Contact Request</h2>
                    <p><strong>Name:</strong> {request.Name}</p>
                    <p><strong>Email:</strong> {request.Email}</p>
                    <p><strong>Company:</strong> {request.Company}</p>
                    <p><strong>Message:</strong></p>
                    <p>{request.Message}</p>"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _settings.Host,
                _settings.Port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _settings.Username,
                _settings.Password);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
