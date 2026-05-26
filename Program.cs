using FluentValidation;
using FluentValidation.AspNetCore;
using IRCM.Data;
using IRCM.Interfaces;
using IRCM.Services.Implementation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================
// DATABASE
// =========================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =========================
// CONTROLLERS
// =========================

builder.Services.AddControllers();

// =========================
// SWAGGER
// =========================

builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddSwaggerGen();

// =========================
// FLUENT VALIDATION
// =========================

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// =========================
// DEPENDENCY INJECTION
// =========================

builder.Services.AddScoped<IAuthService, AuthService>();

// =========================
// CORS
// =========================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// =========================
// BUILD APP
// =========================

var app = builder.Build();

// =========================
// MIDDLEWARE PIPELINE
// =========================

if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();