using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LeadXTechnologiesApi.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Company { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Consent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
