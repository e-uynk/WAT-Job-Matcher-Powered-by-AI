using Microsoft.EntityFrameworkCore;
using WatJobMatcher.Api.Data;
using WatJobMatcher.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// SQLite veritabanı bağlantı ayarı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=watjobmatcher.db"));

// Kural motoru servis kaydı (Dependency Injection)
builder.Services.AddScoped<JobMatcherService>();

// Ollama Parser Servis Kaydı (HttpClient desteğiyle)
builder.Services.AddHttpClient<OllamaParserService>();

// CORS Politikası (Frontend'in 3000 portundan gelen isteklere izin veriyoruz)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104_857_600;
    options.ValueCountLimit = 2048;
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS Middleware (Controller'lardan önce mutlaka eklenmeli)
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();