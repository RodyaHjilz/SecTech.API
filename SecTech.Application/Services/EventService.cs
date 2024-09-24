using Microsoft.EntityFrameworkCore;
using SecTech.Domain.Dto.Attendance;
using SecTech.Domain.Dto.Event;
using SecTech.Domain.Entity;
using SecTech.Domain.Interfaces.Repositories;
using SecTech.Domain.Interfaces.Services;
using SecTech.Domain.Result;

namespace SecTech.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IBaseRepository<Event> _eventRepository;

        public EventService(IBaseRepository<Event> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<BaseResult<Event>> CreateEventAsync(CreateEventDto eventDto)
        {
            if (eventDto == null)
                return new BaseResult<Event>() { ErrorMessage = "Entity is null", ErrorCode = 1 };

            try
            {
                var Event = new Event() 
                {   Address = eventDto.Address,
                    Description = eventDto.Description, 
                    Name = eventDto.Name,
                    Type = eventDto.Type,
                    EventTimeEnd = eventDto.EventTimeEnd,
                    EventTimeStart = eventDto.EventTimeStart
                };
                await _eventRepository.CreateAsync(Event);
                return new BaseResult<Event>() { Data = Event };
            }
            catch (Exception ex)
            {
                return new BaseResult<Event>() { ErrorMessage = ex.Message, ErrorCode = 1 };
            }
        }

        public async Task<BaseResult> DeleteEventAsync(Guid id)
        {
            var evnt = await _eventRepository.GetAll().FirstOrDefaultAsync(e => e.Id == id);
            if (evnt == null)
                return new BaseResult() { ErrorMessage = $"Event with id {id} not found" };
            try
            {
                await _eventRepository.RemoveAsync(evnt);
                return new BaseResult();
            }
            catch (Exception ex)
            {
                return new BaseResult() { ErrorMessage = ex.Message, ErrorCode = 1 };
            }
        }

        public async Task<BaseResult<EventDto>> GetEventByIdAsync(Guid id)
        {
            var evnt = await _eventRepository.GetAll()
                .Include(x => x.Attendances)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (evnt == null)
                return new BaseResult<EventDto>() { ErrorMessage = $"Event with id {id} not found" };

            var eventDto = new EventDto
            {
                Name = evnt.Name,
                Description = evnt.Description,
                Address = evnt.Address,
                Type = evnt.Type,
                EventTimeEnd = evnt.EventTimeEnd,
                EventTimeStart = evnt.EventTimeStart,
                Attendances = evnt.Attendances.Select(a => new AttendanceDto
                {
                    UserName = a.User.FirstName + " " + a.User.LastName,
                    CheckInTime = a.CheckInTime
                }).ToList()
               
            };
            return new BaseResult<EventDto>() { Data = eventDto };

        }
    }
}
