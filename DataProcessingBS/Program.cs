using System.Net.Sockets;
using DataProcessingBS.Data;
using DataProcessingBS.Middleware;
using DataProcessingBS.Services;
using Microsoft.EntityFrameworkCore;
using DataProcessingBS.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddXmlSerializerFormatters(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddHttpClient<TmdbService>();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ApiKeyService>();

builder.Services.AddLogging();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
}

builder.Services.AddDbContext<AppDbcontext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseCors("AllowReactApp");

app.UseMiddleware<ApiKeyMiddleware>();

app.MapRazorPages();

app.UseSwagger();  
app.UseSwaggerUI();  

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();


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
app.UseDeveloperExceptionPage();

app.Run();
