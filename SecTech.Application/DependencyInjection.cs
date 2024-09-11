using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SecTech.Application.Services;
using SecTech.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Application
{
    public static class DependencyInjection
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IQRCodeService, QRCodeService>();
        }

        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(x => x.Cookie.Name = "token")
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
        }


        


    }


}
