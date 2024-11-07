using Homies.Contracts;
using Homies.Data;
using Homies.Data.DataModels;
using Homies.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static Homies.Data.Common.DataConstants;

namespace Homies.Services
{
    public class EventService : IEventService
    {
        private readonly HomiesDbContext context;

        public EventService(HomiesDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task<DetailsEventViewModel> CreateDetailsEventModelAsync(int id)
        {
            var model = await context.Events
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new DetailsEventViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    CreatedOn = e.CreatedOn.ToString(DateTimeFormat),
                    Start = e.Start.ToString(DateTimeFormat),
                    End = e.End.ToString(DateTimeFormat),
                    Organiser = e.Organiser.UserName,
                    Type = e.Type.Name
                })
                .FirstAsync();

            return model;
        }

        public async Task<EventFormModel> CreateEditEventModel(int id)
        {
            var modelToEdit = await context.Events
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EventFormModel()
                {
                    Name = e.Name,
                    Description = e.Description,
                    Start = e.Start.ToString(DateTimeFormat),
                    End = e.End.ToString(DateTimeFormat),
                    TypeId = e.TypeId
                })
                .FirstAsync();

            return modelToEdit;
        }

        public async Task<IEnumerable<JoinedEventViewModel>> CreateJoinedModelAsync(string userId)
        {
            var joinedEvents = await context.EventsParticipants
                .AsNoTracking()
                .Where(ep => ep.HelperId == userId)
                .Select(ep => new JoinedEventViewModel()
                {
                    Id = ep.Event.Id,
                    Name = ep.Event.Name,
                    Start = ep.Event.Start.ToString(DateTimeFormat),
                    Type = ep.Event.Type.Name,
                    Organiser = ep.Event.Organiser.UserName
                })
                .ToListAsync();

            return joinedEvents;
        }

        public async Task CreateNewEventEntityAsync(EventFormModel model, string userId)
        {
            Event eventToAdd = new Event()
            {
                Name = model.Name,
                Description = model.Description,
                OrganiserId = userId,
                CreatedOn = DateTime.Now,
                Start = DateTime.ParseExact(model.Start, DateTimeFormat, CultureInfo.InvariantCulture),
                End = DateTime.ParseExact(model.End, DateTimeFormat, CultureInfo.InvariantCulture),
                TypeId = model.TypeId
            };

            await context.Events.AddAsync(eventToAdd);
            await context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var eventToDelete = await context.Events
                .FirstAsync(e => e.Id == id);

            context.Remove(eventToDelete);
            await context.SaveChangesAsync();
        }

        public async Task EditEventAsync(EventFormModel model, int id)
        {
            var eventToEdit = await context.Events
                .FirstAsync(e => e.Id == id);

            eventToEdit.Name = model.Name;
            eventToEdit.Description = model.Description;
            eventToEdit.Start = DateTime.ParseExact(model.Start, DateTimeFormat, CultureInfo.InvariantCulture);
            eventToEdit.End = DateTime.ParseExact(model.End, DateTimeFormat, CultureInfo.InvariantCulture);
            eventToEdit.TypeId = model.TypeId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllEventViewModel>> GetAllEventsAsync()
        {
            var allEvents = await context.Events
                .AsNoTracking()
                .Select(e => new AllEventViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Start = e.Start.ToString(DateTimeFormat),
                    Type = e.Type.Name,
                    Organiser = e.Organiser.UserName
                })
                .ToListAsync();

            return allEvents;
        }

        public async Task<IEnumerable<TypeViewModel>> GetAllTypesAsync()
        {
            var types = await context.Types
                .AsNoTracking()
                .Select(t => new TypeViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();

            return types;
        }

        public async Task<bool> IsEventExistingAsync(int id)
        {
            var eventToDisplay = await context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventToDisplay == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsUserAuthorised(string userId, int id)
        {
            var eventToCheck = await context.Events
                .AsNoTracking()
                .FirstAsync(e => e.Id == id);

            if (userId != eventToCheck.OrganiserId)
            {
                return false;
            }

            return true;
        }

        public async Task JoinEventAsync(string userId, int id)
        {
            if (!context.EventsParticipants.Any(ep => ep.HelperId == userId && ep.EventId == id))
            {
                await context.EventsParticipants.AddAsync(new EventParticipant()
                {
                    EventId = id,
                    HelperId = userId
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task LeaveEventAsync(string userId, int id)
        {
            if (context.EventsParticipants.Any(ep => ep.HelperId == userId && ep.EventId == id))
            {
                var eventToLeave = await context.EventsParticipants
                    .FirstAsync(ep => ep.HelperId == userId && ep.EventId == id);

                context.EventsParticipants.Remove(eventToLeave);
                await context.SaveChangesAsync();
            }
        }
    }
}
