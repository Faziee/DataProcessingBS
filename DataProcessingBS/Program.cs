using System.Text.Json;
using DataProcessingBS.Data;
using DataProcessingBS.Middleware;
using DataProcessingBS.Modules;
using DataProcessingBS.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services.AddHttpClient<TmdbService>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<ApiKeyService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

builder.Services.AddDbContext<AppDbcontext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseCors("AllowReactApp");
app.UseMiddleware<ApiKeyMiddleware>();

app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.AddExternalMovieEndpoints();
app.AddWatchesStoredProcedureEndpoints();
app.AddWatchListsStoredProcedureEndpoints();
app.AddSubscriptionStoredProcedureEndpoints();
app.AddProfileStoredProcedureEndpoints();
app.AddInvitationsStoredProcedureEndpoints();
app.AddMoviesStoredProcedureEndpoints();
app.AddSeriesStoredProcedureEndpoints();
app.AddAccountStoredProcedureEndpoints();
app.AddEpisodeStoredProcedureEndpoints();
app.AddMediaStoredProcedureEndpoints();
app.AddSubtitlesStoredProcedureEndpoints();
app.AddGenresStoredProcedureEndpoints();

app.MapControllers();

app.Run();