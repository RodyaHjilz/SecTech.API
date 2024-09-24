using SecTech.Domain.Dto.Event;
using SecTech.Domain.Entity;
using SecTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Domain.Interfaces.Services
{
    public interface IEventService
    {
        public Task<BaseResult<Event>> CreateEventAsync(CreateEventDto eventDto);

        public Task<BaseResult<EventDto>> GetEventByIdAsync(Guid id);

        public Task<BaseResult> DeleteEventAsync(Guid id);

    }
}
