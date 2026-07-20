using LeadXTechnologiesApi.DTOs;
using LeadXTechnologiesApi.Models;
using Microsoft.Extensions.Options;
using Resend;

namespace LeadXTechnologiesApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly ResendSettings _settings;

        public EmailService(
            IResend resend,
            IOptions<ResendSettings> settings)
        {
            _resend = resend;
            _settings = settings.Value;
        }

        public async Task SendContactEmailAsync(ContactRequestDto request)
        {
            var message = new EmailMessage();

            message.From = _settings.FromEmail;
            message.To.Add(_settings.ToEmail);

            message.Subject = "New LeadX Technologies Contact Inquiry";

            message.HtmlBody = $@"
            <h2>New Contact Request</h2>

            <p><strong>Name:</strong> {request.Name}</p>
            <p><strong>Email:</strong> {request.Email}</p>
            <p><strong>Company:</strong> {request.Company}</p>
            <p><strong>Message:</strong></p>
            <p>{request.Message}</p>";

            var result = await _resend.EmailSendAsync(message);
        }
    }
}
