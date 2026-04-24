using ExcursionManager.Application.Interfaces;
using ExcursionManager.Application.Services;
using ExcursionManager.Domain.Entities;
using ExcursionManager.Domain.Interfaces;
using ExcursionManager.Persistence.Context;
using ExcursionManager.Persistence.Repositories;
using DomainRoute = ExcursionManager.Domain.Entities.Route;

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

// ─── Services ─────────────────────────────
builder.Services.AddScoped<IExcursionService, ExcursionService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// ─── CORS - allow React frontend ──────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
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
app.UseCors("AllowReact");
app.UseAuthorization();
app.MapControllers();

app.Run();