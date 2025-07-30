using MediatR;
using Microsoft.Extensions.Caching.Memory;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, bool>
{
    private readonly IEventRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<DeleteEventCommandHandler> _logger;

    public DeleteEventCommandHandler(IEventRepository repository, IMemoryCache cache, ILogger<DeleteEventCommandHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _repository.GetEventById(request.EventId);
        if (eventEntity == null || eventEntity.IsDeleted)
        {
            return false;
        }
        if (eventEntity.CreatorUserId != request.RequestingUserId)
        {
            _logger.LogWarning("[LOG] Unauthorized silme işlemi girişimi. EventId: {EventId}", request.EventId);
            return false;
        }
        eventEntity.IsDeleted = true;

        await _repository.UpdateAsync(eventEntity);
        _logger.LogInformation("[LOG] Event silindi.");
        _cache.Remove("GetAllEvents");
        _logger.LogInformation("[CACHE] Cache temizlendi.");
        return true;
    }
}