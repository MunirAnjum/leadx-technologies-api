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

            try
            {
                var result = await _resend.EmailSendAsync(message);
                Console.WriteLine($"Resend success: {result.Content}");
            }
            catch (ResendException rex)
            {
                Console.WriteLine($"Resend error: {rex.Message} | Status: {rex.StatusCode}");
                throw;
            }

        }
    }

    //public class EmailService : IEmailService
    //{
    //    public readonly EmailSettings _settings;
    //    public EmailService(IOptions<EmailSettings> settings)
    //    {
    //        _settings = settings.Value;       
    //    }
    //    public async Task SendContactEmailAsync(ContactRequestDto request)
    //    {
    //        var email = new MimeMessage();
    //        email.From.Add(
    //            new MailboxAddress(
    //                _settings.SenderName,
    //                _settings.SenderEmail
    //            ));
    //        email.To.Add(
    //            MailboxAddress.Parse(_settings.ReceiverEmail));

    //        email.Subject = "New LeadX Technologies Contact Inquiry";

    //        email.Body = new TextPart("html")
    //        {
    //            Text = $@"
    //                <h2>New Contact Request</h2>
    //                <p><strong>Name:</strong> {request.Name}</p>
    //                <p><strong>Email:</strong> {request.Email}</p>
    //                <p><strong>Company:</strong> {request.Company}</p>
    //                <p><strong>Message:</strong></p>
    //                <p>{request.Message}</p>"
    //        };

    //        using var smtp = new SmtpClient();

    //        await smtp.ConnectAsync(
    //            _settings.Host,
    //            _settings.Port,
    //            SecureSocketOptions.StartTls);

    //        await smtp.AuthenticateAsync(
    //            _settings.Username,
    //            _settings.Password);

    //        await smtp.SendAsync(email);
    //        await smtp.DisconnectAsync(true);
    //    }
    //}
}
