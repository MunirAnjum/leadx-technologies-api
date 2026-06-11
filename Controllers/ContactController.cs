using LeadXTechnologiesApi.Data;
using LeadXTechnologiesApi.Models;
using LeadXTechnologiesApi.DTOs;
using LeadXTechnologiesApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace LeadXTechnologiesApi.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public ContactController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Send(ContactRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Save to database
            var message = new ContactMessage
            {
                Name = request.Name,
                Email = request.Email,
                Company = request.Company,
                Message = request.Message,
                Consent = request.Consent
            };

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            // Send Email
            await _emailService.SendContactEmailAsync(request);
            return Ok(new
            {
                message = "Contact Save and Email Send Successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.ContactMessages
                .FindAsync(id);

            if (contact == null)
            {
                return NotFound(
                    new
                    {
                        message = "Contact not found"
                    });
            }

            _context.ContactMessages.Remove(contact);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.ContactMessages.ToList());
        }
    }
}
