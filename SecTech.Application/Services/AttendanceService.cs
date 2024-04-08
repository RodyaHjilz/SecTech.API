using Microsoft.EntityFrameworkCore;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Repositories;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IBaseRepository<Attendance> _attendanceRepository;

        public AttendanceService(IBaseRepository<Attendance> attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<BaseResult<Attendance>> CheckInAsync(Guid userId, Guid eventId)
        {
            try
            {

                var isUserCheckedIn = _attendanceRepository.GetAll().Any(x => x.UserId == userId && x.EventId == eventId);
                if(isUserCheckedIn)
                {
                    return new BaseResult<Attendance>()
                    {
                        ErrorMessage = "User is already checked in"
                    };
                }

                var attendance = new Attendance()
                {
                    UserId = userId,
                    EventId = eventId,
                    CheckInTime = DateTime.UtcNow
                };
                await _attendanceRepository.CreateAsync(attendance);

                return new BaseResult<Attendance>() { Data = attendance };
            }
            catch (Exception ex)
            {
                return new BaseResult<Attendance>() { ErrorMessage = ex.Message, ErrorCode = 1 };
            }
        }

        public Task<BaseResult<Attendance>> CheckOutAsync(Guid userId, Guid eventId)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResult<Attendance>> GetAttendanceAsync(int attendanceId)
        {
            var attendance = await _attendanceRepository.GetAll().FirstOrDefaultAsync(e => e.Id == attendanceId);
            if (attendance == null)
                return new BaseResult<Attendance>() 
                { ErrorMessage = $"Attendance with id {attendanceId} is not found" };

            return new BaseResult<Attendance>() { Data = attendance };
        }

        public async Task<BaseResult<IEnumerable<Attendance>>> GetUserAttendancesAsync(Guid userId)
        {
            var attendances = await _attendanceRepository.GetAll().Where(x => x.UserId == userId).ToListAsync();
            if (attendances == null)
                return new BaseResult<IEnumerable<Attendance>>()
                { ErrorMessage = $"There is no user attendances", ErrorCode = 1};



            return new BaseResult<IEnumerable<Attendance>>() { Data = attendances };
        }

        public async Task<BaseResult<bool>> IsUserCheckedInAsync(Guid userId, Guid eventId)
        {
            try
            {
                var attendance = await _attendanceRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.EventId == eventId);
                if (attendance != null)
                    return new BaseResult<bool>() { Data = true };

                else
                    return new BaseResult<bool>() { Data = false };
            }
            catch (Exception ex)
            {
                return new BaseResult<bool>() { ErrorMessage = ex.Message };
            }
        }
    }
}
