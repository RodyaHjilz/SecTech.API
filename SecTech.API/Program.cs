using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecTech.Application;
using SecTech.DAL.Infrastructure.DependencyInjection;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Simple QR-code secure system",
        Description = "Пример ASP .NET Core Web API",
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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(x => x.Cookie.Name = "token")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = "SecTech",            
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = "SecTech",
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")),
            // валидация ключа безопасности
             ValidateIssuerSigningKey = true,
        };

        options.Events = new JwtBearerEvents
        {
           OnMessageReceived = context =>
           {
               context.Token = context.Request.Cookies["token"];
               return Task.CompletedTask;
           }
        };

    });


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
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
