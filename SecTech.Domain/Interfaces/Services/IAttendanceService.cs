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
        public Task<BaseResult<Attendance>> CheckInAsync(Guid userId, Guid eventId);
        public Task<BaseResult<Attendance>> CheckOutAsync(Guid userId, Guid eventId);
        public Task<BaseResult<bool>> IsUserCheckedInAsync(Guid userId, Guid eventId);
        public Task<BaseResult<Attendance>> GetAttendanceAsync(int attendanceId);
        public Task<BaseResult<IEnumerable<Attendance>>> GetUserAttendancesAsync(Guid userId);
    }
}
