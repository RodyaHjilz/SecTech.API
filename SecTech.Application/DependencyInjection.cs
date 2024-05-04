using Microsoft.Extensions.DependencyInjection;
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
        }
    }


}
