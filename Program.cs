using LeadXTechnologiesApi.Models;
using LeadXTechnologiesApi.Services;
using LeadXTechnologiesApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Resend;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null
            );
        });
});

// Email
//builder.Services.Configure<EmailSettings>(
//    builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<ResendSettings>(
    builder.Configuration.GetSection("Resend"));

builder.Services.AddResend(options =>
{
    options.ApiToken = builder.Configuration["Resend:ApiKey"]!;
});

builder.Services.AddScoped<IEmailService, EmailService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "https://localhost:5173",
            "https://project-25pb2.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// JWT
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

var app = builder.Build();
foreach (var kv in app.Configuration.AsEnumerable())
{
    Console.WriteLine($"{kv.Key} = {kv.Value}");
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("DB Migration failed: " + ex.Message);
    }
}

Console.WriteLine(
    Environment.GetEnvironmentVariable("Resend__ApiKey"));

Console.WriteLine(
    Environment.GetEnvironmentVariable("Resend__FromEmail"));

Console.WriteLine(
    Environment.GetEnvironmentVariable("Resend__ToEmail"));

app.UseStaticFiles();

app.MapControllers();

app.MapGet("/", () => "LeadX API is running 🚀");

app.Run();