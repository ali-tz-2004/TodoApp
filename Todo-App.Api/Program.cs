using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(); // Scalar will use this

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();              // for openapi.json
    app.MapScalarApiReference();   // for Scalar UI
}

app.UseHttpsRedirection();

app.MapControllers(); // Use attribute-based controllers

app.Run();