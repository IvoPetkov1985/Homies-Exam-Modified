using Homies.Models;

namespace Homies.Contracts
{
    public interface IEventService
    {
        Task<DetailsEventViewModel> CreateDetailsEventModelAsync(int id);

        Task<EventFormModel> CreateEditEventModel(int id);

        Task<IEnumerable<JoinedEventViewModel>> CreateJoinedModelAsync(string userId);

        Task CreateNewEventEntityAsync(EventFormModel model, string userId);

        Task DeleteEventAsync(int id);

        Task EditEventAsync(EventFormModel model, int id);

        Task<IEnumerable<AllEventViewModel>> GetAllEventsAsync();

        Task<IEnumerable<TypeViewModel>> GetAllTypesAsync();

        Task<bool> IsEventExistingAsync(int id);

        Task<bool> IsUserAuthorised(string userId, int id);

        Task JoinEventAsync(string userId, int id);

        Task LeaveEventAsync(string userId, int id);
    }
}
