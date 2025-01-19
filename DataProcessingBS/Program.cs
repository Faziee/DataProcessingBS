using DataProcessingBS.Data;
using DataProcessingBS.Middleware;
using DataProcessingBS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DataProcessingBS.Contracts;
using DataProcessingBS.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddXmlSerializerFormatters();  // Allows XML responses alongside JSON
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddScoped<ApiKeyService>();

builder.Services.AddSwaggerGen();

// // Add Swagger with API Key Authentication
// builder.Services.AddSwaggerGen(c =>
// {
//     c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
//     {
//         In = ParameterLocation.Header,
//         Name = "Api-Key",  // Name of the header
//         Type = SecuritySchemeType.ApiKey,
//         Description = "Enter your API key"
//     });
//
//     // Enforce the API Key requirement for all routes
//     c.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "ApiKey"
//                 }
//             },
//             new List<string>()
//         }
//     });
// });

// CORS setup for React app, will be used eventually to connect our front end
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5174")  
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
}

builder.Services.AddDbContext<AppDbcontext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Middleware configuration, commented out since Api Key authentication is not fully implemented yet
//app.UseMiddleware<ApiKeyMiddleware>();

// CORS policy application
app.UseCors("AllowReactApp");

app.UseSwagger();  
app.UseSwaggerUI();  // Swagger UI for API documentation

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.AddAccountStoredProcedureEndpoints();

app.Run();
