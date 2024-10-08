﻿using Microsoft.EntityFrameworkCore;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Dto.User;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Repositories;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;
using SecTech.Domain.Enums;
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
        private readonly IBaseRepository<Event> _eventRepository;
        private readonly IBaseRepository<User> _userRepository;
        public AttendanceService(IBaseRepository<Attendance> attendanceRepository, IBaseRepository<Event> eventRepository, IBaseRepository<User> userRepository)
        {
            _attendanceRepository = attendanceRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResult<Attendance>> CheckInAsync(Guid userId, Guid eventId)
        {
            try
            {
                var ev = await _eventRepository.GetAll().FirstOrDefaultAsync(x=>x.Id == eventId);
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x=>x.Id == userId);

                if(ev == null) { return new BaseResult<Attendance>() { ErrorMessage = "Event not found" }; }
                if(user == null) { return new BaseResult<Attendance>() { ErrorMessage = "User not found" }; }
                
                // Если событие доступно только определенным группам, то проверить наличие группы у user
                if(ev.Type == EventType.EVENT_GROUPACCESSED)
                {
                    bool hasMatchingGroup = false;
                    if (ev.AccessedGroups != null && user.Groups != null)
                        hasMatchingGroup = ev.AccessedGroups.Any(ga => user.Groups.Any(ug=> ug.Id == ga.Id));

                    if(!hasMatchingGroup) { return new BaseResult<Attendance>() { ErrorMessage = "User has no access [GROUP]" }; }
                }

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

        public async Task<BaseResult<IEnumerable<AttendedUserDto>>> GetEventAttendancesAsync(Guid eventId)
        {
            try
            {
                var attendances = await _attendanceRepository
                    .GetAll()
                    .Where(x => x.EventId == eventId)
                    .Include(x => x.User)
                    .ToListAsync();
                if (attendances == null)
                    return new BaseResult<IEnumerable<AttendedUserDto>>()
                    { ErrorMessage = $"There is no user attendances", ErrorCode = 1 };
                var users = attendances.Select(x => new AttendedUserDto()
                {
                    Id = x.UserId,
                    Name = (x.User.FirstName != null) ? x.User.FirstName + x.User.LastName : "Без имени",
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber != null ? x.User.PhoneNumber : "Без номера телефона",
                    CheckInTime = x.CheckInTime
                });
                
                return new BaseResult<IEnumerable<AttendedUserDto>>() { Data = users };
            }
            catch(Exception ex)
            {
                return new BaseResult<IEnumerable<AttendedUserDto>>()
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
        }

        public async Task<BaseResult<IEnumerable<AttendedEventDto>>> GetUserAttendancesAsync(Guid userId)
        {
            try
            {
                var attendances = await _attendanceRepository
                    .GetAll()
                    .Where(x => x.UserId == userId)
                    .Include(x => x.Event)
                    .ToListAsync();

                if (attendances == null)
                    return new BaseResult<IEnumerable<AttendedEventDto>>()
                    {
                        ErrorMessage = "There is no user attended this event",
                        ErrorCode = 1
                    };
                var eventList = attendances.Select(x => new AttendedEventDto()
                {
                    Id = x.EventId,
                    Name = x.Event.Name,
                    Description = x.Event.Description,
                    Address = x.Event.Address
                });
                return new BaseResult<IEnumerable<AttendedEventDto>>() { Data = eventList };

            } catch (Exception ex)
            {
                return new BaseResult<IEnumerable<AttendedEventDto>>()
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
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
