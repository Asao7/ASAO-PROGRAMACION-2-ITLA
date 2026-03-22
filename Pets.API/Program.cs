using Microsoft.EntityFrameworkCore;
using Pets.Infrastructure.Context;
using Pets.Infrastructure.Repositories;
using Pets.Domain.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PetsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPetRepository, PetRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();