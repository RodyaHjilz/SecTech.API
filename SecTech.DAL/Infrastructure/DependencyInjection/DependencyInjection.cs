using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecTech.DAL.Repositories;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Repositories;


namespace SecTech.DAL.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDataAccessLayer(this IServiceCollection services)
        {
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SecTech;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
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
        }
    }
}
