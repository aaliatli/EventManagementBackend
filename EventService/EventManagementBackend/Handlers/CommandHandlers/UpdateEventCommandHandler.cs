using EventService.Data;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly IEventRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<UpdateEventCommandHandler> _logger;

    public UpdateEventCommandHandler(IEventRepository repository, IMemoryCache cache, ILogger<UpdateEventCommandHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _repository.GetEventById(request.Id);
        if (existingEvent == null)
        {
            throw new Exception("Etkinlik bulunamadı");
        }
        existingEvent.Title = request.Title;
        existingEvent.Location = request.Location;
        existingEvent.StartDate = request.StartDate;
        existingEvent.EndDate = request.EndDate;
        existingEvent.Capacity = request.Capacity;

        await _repository.UpdateAsync(existingEvent);

        _logger.LogInformation("[LOG] Event güncellendi.");
        _cache.Remove("GetAllEvents");
        _logger.LogInformation("[CACHE] Cache temizlendi.");

        return Unit.Value;
    }   
}