using Scalar.AspNetCore;
using TodoApp.Api;
using TodoApp.Application.Services.Auth.Command;
using TodoApp.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(); 
builder.Services.AddDatabaseConfig(builder.Configuration);

builder.Services.Configure<Configs>(builder.Configuration.GetSection("Configs"));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();             
    app.MapScalarApiReference();  
}

app.UseHttpsRedirection();

app.MapControllers(); 

app.Run();