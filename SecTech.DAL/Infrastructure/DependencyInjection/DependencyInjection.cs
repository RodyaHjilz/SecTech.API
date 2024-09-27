using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecTech.DAL.Repositories;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Repositories;


namespace SecTech.DAL.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.InitRepositories();
        }

        private static void InitRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<IBaseRepository<Attendance>, BaseRepository<Attendance>>();
            services.AddScoped<IBaseRepository<Event>, BaseRepository<Event>>();
            services.AddScoped<IBaseRepository<UGroup>, BaseRepository<UGroup>>();
        }
    }
}
