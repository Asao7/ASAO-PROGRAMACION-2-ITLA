using ExcursionManager.API.Services;
using ExcursionManager.Application.Interfaces;
using ExcursionManager.Application.Services;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;
using ExcursionManager.Persistence.Context;
using ExcursionManager.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ─── Database connection ───────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DatabaseContext(connectionString!));

// ─── Repositories ─────────────────────────
builder.Services.AddScoped<IRepository<ExcursionManager.Domain.Entities.Route>, RouteRepository>();
builder.Services.AddScoped<IRepository<Guide>, GuideRepository>();
builder.Services.AddScoped<IRepository<Participant>, ParticipantRepository>();
builder.Services.AddScoped<IRepository<Excursion>, ExcursionRepository>();
builder.Services.AddScoped<IRepository<Reservation>, ReservationRepository>();
builder.Services.AddScoped<UserRepository>();

// ─── Services ─────────────────────────────
builder.Services.AddScoped<IExcursionService, ExcursionService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ─── JWT ───────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ─── CORS - allow React frontend ──────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5175")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// activar CORS
app.UseCors("AllowReact");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();