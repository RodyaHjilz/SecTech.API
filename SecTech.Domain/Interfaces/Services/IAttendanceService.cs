using SecTech.Domain.Dto.Event;
using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
using SecTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Domain.Interfaces.Services
{
    public interface IAttendanceService
    {
        /// <summary>
        /// Зарегистрировать посещение пользователя к событию
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Task<BaseResult<Attendance>> CheckInAsync(Guid userId, Guid eventId);
        public Task<BaseResult<Attendance>> CheckOutAsync(Guid userId, Guid eventId);
        /// <summary>
        /// Проверить, посетил ли пользователь событие
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Task<BaseResult<bool>> IsUserCheckedInAsync(Guid userId, Guid eventId);
        public Task<BaseResult<Attendance>> GetAttendanceAsync(int attendanceId);
        public Task<BaseResult<IEnumerable<AttendedEventDto>>> GetUserAttendancesAsync(Guid userId);
        public Task<BaseResult<IEnumerable<AttendedUserDto>>> GetEventAttendancesAsync(Guid eventId);
    }
}
