using EventService.Data;
using EventService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class EventRepository : IEventRepository
{
    private readonly EventDbContext _context;

    public EventRepository(EventDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Event entity)
    {
        await _context.Events.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event entity)
    {
        _context.Events.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<List<Event>> GetCurrentEventsAsync()
    {
        
        return await _context.Events.Where(e => e.StartDate >= DateTime.Today).Select(e => new Event { Title = e.Title}).ToListAsync();
    }
    public async Task<Event> GetEventById(Guid Id)
    {
        return await _context.Events.FindAsync(Id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Events.FindAsync(id);
        if (entity == null) return false;

        _context.Events.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Event>> GetSearchedEvents(){
        return await _context.Events.ToListAsync();
    }
} 