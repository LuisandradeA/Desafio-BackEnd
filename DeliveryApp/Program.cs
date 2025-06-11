using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.DTOs.Request;
using DeliveryApp.Application.Services;
using DeliveryApp.Application.Validators;
using DeliveryApp.Application.Validators.Requests;
using DeliveryApp.Domain.Interfaces;
using DeliveryApp.Infrastructure.Persistence.Contexts;
using DeliveryApp.Infrastructure.Repositories;
using DeliveryApp.Services.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DeliveryAppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Register application services
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();

// Register repositories
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();

//Register Validators
// Registrar o FluentValidation (testar depois)
//builder.Services.AddFluentValidationAutoValidation(); // Habilita validação automática nos controllers
builder.Services.AddScoped<IValidator<CreateMotorcycleDTO>, CreateMotorcycleValidator>();
builder.Services.AddScoped<IValidator<UpdateLicensePlateDTO>, UpdateLicensePlateValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
