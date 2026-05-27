using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using IRCM.Data;
using IRCM.Helpers;
using IRCM.Interfaces;
using IRCM.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
// JWT AUTHENTICATION
// =========================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!
                )
            )
        };
});

// =========================
// DEPENDENCY INJECTION
// =========================

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<JwtHelper>();

builder.Services.AddScoped<
    IPropertyService,
    PropertyImplementation
>();

builder.Services.AddScoped<
    ILeaseRequestService,
    LeaseRequestImplementation
>();

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
    // app.UseSwagger();

    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// =========================
// AUTH MIDDLEWARE
// =========================

app.UseAuthentication();

app.UseAuthorization();

// =========================
// MAP CONTROLLERS
// =========================

app.MapControllers();

// =========================
// RUN APP
// =========================

app.Run();