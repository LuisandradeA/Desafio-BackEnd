using DeliveryApp.Application.DTOs.Messaging;
using DeliveryApp.CreatedMotorcycleEventConsumer;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using DeliveryApp.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

//builder.Services.AddScoped<IMongoRepository<Motorcycle>, MongoRepository<Motorcycle>>();

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetValue<string>("MongoDB:ConnectionString");
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var databaseName = configuration.GetValue<string>("MongoDB:DatabaseName");
    return client.GetDatabase(databaseName);
});

// Para registrar um repositório específico (ex: para entidade 'Motorcycle')
builder.Services.AddSingleton<IMongoRepository<CreatedMotorcycleEventMessage>>(provider =>
{
    var database = provider.GetRequiredService<IMongoDatabase>();

    // Forçar a criação da coleção se não existir
    var collectionName = "CreatedMotorcycleEvents"; // ou pegue do configuration
    var collectionExists = database.ListCollectionNames(new ListCollectionNamesOptions
    {
        Filter = new BsonDocument("name", collectionName)
    }).Any();

    if (!collectionExists)
    {
        database.CreateCollection(collectionName);
    }

    return new MongoRepository<CreatedMotorcycleEventMessage>(database);
});
//builder.Services.AddScoped<IMongoRepository<Motorcycle>, MongoRepository<Motorcycle>>();


var host = builder.Build();
host.Run();
