using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.DTOs.Request;
using DeliveryApp.Application.DTOs.Request.Driver;
using DeliveryApp.Application.DTOs.Request.Rentals;
using DeliveryApp.Application.Services;
using DeliveryApp.Application.Validators;
using DeliveryApp.Application.Validators.Requests;
using DeliveryApp.Domain.Interfaces;
using DeliveryApp.Infrastructure.Persistence.Contexts;
using DeliveryApp.Infrastructure.Repositories;
using DeliveryApp.Services.Services;
using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Serialization.SystemTextJson;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IRentalService, RentalService>();

builder.Services.AddSingleton<IMessageBusService, MessageBusService>();


// Register repositories
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

//Register Validators
// Registrar o FluentValidation (testar depois)
//builder.Services.AddFluentValidationAutoValidation(); // Habilita validação automática nos controllers
builder.Services.AddScoped<IValidator<CreateMotorcycleDTO>, CreateMotorcycleValidator>();
builder.Services.AddScoped<IValidator<UpdateLicensePlateDTO>, UpdateLicensePlateValidator>();
builder.Services.AddScoped<IValidator<CreateDriverDTO>, CreateDriverValidator>();
builder.Services.AddScoped<IValidator<CreateRentalDTO>, CreateRentalValidator>();


var app = builder.Build();

// APLICA AS MIGRATIONS AUTOMATICAMENTE
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DeliveryAppDbContext>();
    db.Database.Migrate(); // <--- aplica as migrations aqui
}

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
