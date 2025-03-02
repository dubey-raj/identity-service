using IdentityService.Configurations;
using IdentityService.Model;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog.Formatting.Json;
using Serilog;
using IdentityService.Middlewares;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions.Core;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logConfig =>
{
    logConfig.ClearProviders();
    logConfig.AddDebug();
    logConfig.AddEventSourceLogger();
    ConfigureLogging(builder);
    logConfig.AddSerilog();
});
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUsersDBContext(builder.Configuration);
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(policy => policy
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);
app.UseSerilogRequestLogging();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

static void ConfigureLogging(WebApplicationBuilder builder)
{
    //Configure serilog as logger
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console(new JsonFormatter(renderMessage: false))
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers())
        .Enrich.With(new ExceptionEnricher())
        .CreateLogger();
    builder.Host.UseSerilog();
}

class ExceptionEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent.Exception == null)
            return;

        var logEventProperty = propertyFactory.CreateProperty("EscapedException", logEvent.Exception.ToString().Replace("\r\n", "\\r\\n"));
        logEvent.AddPropertyIfAbsent(logEventProperty);
    }
}