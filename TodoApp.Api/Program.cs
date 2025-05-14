using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using TodoApp.Api.BehaviorHandler;
using TodoApp.Api.ExceptionHandler;
using TodoApp.Application.Auth.Commands.RegisterCommand;
using TodoApp.Infrastructure.Models;

namespace TodoApp.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddAppServices(builder.Configuration);
        
        if (!builder.Environment.IsEnvironment("Test"))
        {
            ConfigureDatabase(builder.Services, builder.Configuration);
        }

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
    }
    
    public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (env == "Test")
            return;

        services.AddDatabaseConfig(configuration);
    }
}
