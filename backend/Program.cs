using Microsoft.EntityFrameworkCore;
using PerfumeShopAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PerfumeShopAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

// Database
builder.Services.AddDbContext<PerfumeShopeContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PerfumeShopDB")));

// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// OpenAPI
builder.Services.AddOpenApi();


// JWT settings
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// OpenAPI + Swagger UI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "PerfumeShopAPI v1");
    });
}

// Global exception handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionFeature =
            context.Features.Get<
                Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        var exception = exceptionFeature?.Error;

        Console.WriteLine($"Unhandled exception: {exception}");

        await context.Response.WriteAsJsonAsync(new
        {
            error = "An unexpected error occurred. Please try again later."
        });
    });
});

// CORS BEFORE HTTPS redirection
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

// JWT Authentication
app.UseAuthentication();

// Authorization
app.UseAuthorization();

// API Controllers
app.MapControllers();

app.Run();
