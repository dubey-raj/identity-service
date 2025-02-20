using IdentityService.Configurations;
using IdentityService.Model;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog.Formatting.Json;
using Serilog;
using IdentityService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging(builder);
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
        .WriteTo.Console(new JsonFormatter(renderMessage: true))
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog();
}