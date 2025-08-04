using EventService.Models;

public interface IEventRepository
{
    Task AddAsync(Event entity);
    Task UpdateAsync(Event entity);
    Task<List<Event>> GetAllEventsAsync();
    Task<List<Event>> GetCurrentEventsAsync();
    Task<Event> GetEventById(Guid Id);
    Task<bool> DeleteAsync(Guid id);
    Task<List<Event>> GetSearchedEvents();
}