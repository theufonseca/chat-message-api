using Infra.MySql;
using Infra.RabbitMQ;
using Infra.RabbitMQ.Consumers;
using MediatR;
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

//rabbitconfig
var rabbitSection = builder.Configuration.GetSection("RabbitMQ");
var rabbitSettings = new RabbitMQSettings();
rabbitSection.Bind(rabbitSettings);
builder.Services.AddSingleton<RabbitMQSettings>(rabbitSettings);
builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
{
    HostName = rabbitSettings.HostName,
    Port = 15672,
    UserName = "guest",
    Password = "guest",
    DispatchConsumersAsync = true
});
builder.Services.AddSingleton<ModelFactory>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<ModelFactory>().CreateChannel());
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
