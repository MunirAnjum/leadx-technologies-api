namespace LeadXTechnologiesApi.Models
{
    public class ResendSettings
    {
        public string ApiKey { get; set; } = string.Empty;

        public string FromEmail { get; set; } = string.Empty;

        public string ToEmail { get; set; } = string.Empty;
    }
}