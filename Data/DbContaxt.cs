using Microsoft.EntityFrameworkCore;
using LeadXTechnologiesApi.Models;

namespace LeadXTechnologiesApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
}