using EventService.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Unit>
{
    private readonly IEventRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CreateEventCommandHandler> _logger;

    public CreateEventCommandHandler(IEventRepository repository, IMemoryCache cache, ILogger<CreateEventCommandHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Location = request.Location,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Capacity = request.Capacity,
            AgeRestriction = request.AgeRestriction > 0 ? request.AgeRestriction : 0,
            CreatorUserId = request.CreatorUserId

        };
        
        await _repository.AddAsync(newEvent);

        _logger.LogInformation("[LOG] Event Eklendi");
        _cache.Remove("GetAllEvents");
        _logger.LogInformation("[CACHE] Cache temizlendi.");

        return Unit.Value;
    }

}