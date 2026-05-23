using LeadXTechnologiesApi.DTOs;
using LeadXTechnologiesApi.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
namespace LeadXTechnologiesApi.Services
{
    public interface IEmailService
    {
        Task SendContactEmailAsync(ContactRequestDto request);
    }
}
