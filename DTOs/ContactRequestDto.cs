using System.ComponentModel.DataAnnotations;
namespace LeadXTechnologiesApi.DTOs
{
    public class ContactRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? Company { get; set; }
        [Required]
        public string Message { get; set; } = string.Empty;
        [Required]
        public bool Consent { get; set; }

    }
}
