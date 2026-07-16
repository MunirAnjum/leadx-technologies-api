using LeadXTechnologiesApi.DTOs;
using LeadXTechnologiesApi.Models;
using Microsoft.Extensions.Options;
namespace LeadXTechnologiesApi.Services
{
    public interface IEmailService
    {
        Task SendContactEmailAsync(ContactRequestDto request);
    }
}
