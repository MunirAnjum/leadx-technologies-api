using LeadXTechnologiesApi.Models;
using LeadXTechnologiesApi.Services;
using LeadXTechnologiesApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// OpenAPI / Swagger (optional but useful)
//builder.Services.AddOpenApi();

//DbContext configure
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Email configuration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Dependency Injection
builder.Services.AddScoped<IEmailService, EmailService>();

// CORS (allow frontend React/Vue)
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var jwt = builder.Configuration.GetSection("Jwt");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        )
    };
});

Console.WriteLine(
    BCrypt.Net.BCrypt.HashPassword("Leadx@123")
);
var app = builder.Build();

// Enable OpenAPI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

// Enable CORS BEFORE authorization
app.UseCors("FrontendPolicy");

app.UseAuthentication();
// Authorization not required for contact form
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();