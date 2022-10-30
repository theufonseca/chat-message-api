using Domain.Interfaces;
using Infra.gRPC;
using Infra.MySql;
using Infra.MySql.Services;
using Infra.RabbitMQ;
using Infra.RabbitMQ.Consumers;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddTransient<ProfileClient>();
builder.Services.AddTransient<IProfileService, ProfileService>();

//rabbit config
var configSection = builder.Configuration.GetSection("RabbitMQ");
var settings = new RabbitMQSettings();
configSection.Bind(settings);
builder.Services.AddSingleton<RabbitMQSettings>(settings);

builder.Services.AddSingleton<IConnectionFactory>(x => new ConnectionFactory
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
});

builder.Services.AddSingleton<RabbitModelFactory>();
builder.Services.AddSingleton(x => x.GetRequiredService<RabbitModelFactory>().CreateChannel());
builder.Services.AddHostedService<MessageReceiver>();

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
