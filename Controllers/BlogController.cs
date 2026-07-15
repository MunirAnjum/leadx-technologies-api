using LeadXTechnologiesApi.Data;
using LeadXTechnologiesApi.Models;
using LeadXTechnologiesApi.DTOs;
using LeadXTechnologiesApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace LeadXTechnologiesApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BlogController : ControllerBase
    {
        public readonly AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(BlogDto post)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            var blogPost = new BlogPost
            {
                Title = post.Title,
                Slug = GenerateSlug(post.Title),
                Summary = post.Summary,
                Content = post.Content,
                Author = string.IsNullOrWhiteSpace(post.Author) ? "LeadX Team" : post.Author,
                Category = post.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            return Ok(blogPost);
        }

        private string GenerateSlug(string title)
        {
            return title.Trim().ToLower().Replace(" ", "-");
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _context.BlogPosts.ToListAsync();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog == null)
                return NotFound();
            return Ok(blog);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, BlogDto post)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog == null)
                return NotFound();

            blog.Title = post.Title;
            blog.Category = post.Category;
            blog.Author = post.Author;
            blog.Summary = post.Summary;
            blog.Content = post.Content;
            blog.CreatedAt = post.CreatedAt;

            await _context.SaveChangesAsync();

            return Ok(blog);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.BlogPosts
                .FindAsync(id);

            if(blog == null)
            {
                return NotFound(
                    new
                    {
                        message = "Blog not found"
                    });
            }

            _context.BlogPosts.Remove(blog);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}