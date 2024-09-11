using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using SecTech.Application;
using SecTech.DAL.Infrastructure.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
// builder.Logging.SetMinimumLevel(LogLevel.Debug); // Минимальный уровень логгирования

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Simple QR-code secure system",
        Description = "Пет-проект для МКБ",
        Contact = new OpenApiContact
        {
            Name = "tg: @bobach4",
            Url = new Uri("https://t.me/bobach4")
        }
    });

    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "SecTech.API.xml");
    options.IncludeXmlComments(xmlPath);

});
builder.Services.AddDataAccessLayer();
builder.Services.AddServices();
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddCheck("self", () => HealthCheckResult.Healthy("Server is working"))
    .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 500, name: "memory")
    .AddDiskStorageHealthCheck(setup => setup.AddDrive("C:\\", minimumFreeMegabytes: 1000), name: "disk_storage");

builder.Services.AddJwtAuthentication();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://localhost:5173").AllowAnyHeader().AllowAnyMethod();
        });
});


var app = builder.Build();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
