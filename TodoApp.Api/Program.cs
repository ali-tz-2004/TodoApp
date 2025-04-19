using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using TodoApp.Api;
using TodoApp.Api.BehaviorHandler;
using TodoApp.Api.ExceptionHandler;
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

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(SetUserIdBehavior<,>));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Configs:TokenIssuer"],
            ValidAudience = builder.Configuration["Configs:TokenAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Configs:TokenKey"]))
        };
    });


builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();             
    app.MapScalarApiReference();  
}


app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); 

app.Run();