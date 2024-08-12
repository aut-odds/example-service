using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;
using Scrutor;
using ExampleService.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    })
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    );

builder.Services
    .Scan(selector => selector
    .FromCallingAssembly()
    .AddClasses(tf => tf.Where(t => t.Name.EndsWith("Repository")))
        .UsingRegistrationStrategy(RegistrationStrategy.Throw)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    .AddClasses(tf => tf.Where(t => t.Name.EndsWith("Service")))
        .UsingRegistrationStrategy(RegistrationStrategy.Throw)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    );

builder.Services.Configure<ExampleDatabase>(builder.Configuration.GetSection("ExampleDatabase"));

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var exampleDatabase = sp.GetRequiredService<IOptions<ExampleDatabase>>().Value;
    var mongoClient = new MongoClient(exampleDatabase.ConnectionString);
    return mongoClient.GetDatabase(exampleDatabase.DatabaseName);
});

var convention = new SnakeCaseElementNameConvention();
ConventionRegistry.Register(convention.Name, new ConventionPack { convention }, _ => true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
