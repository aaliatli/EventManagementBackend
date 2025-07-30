using MediatR;
using Microsoft.Extensions.Caching.Memory;

public class GetCurrentEventQueryHandler : IRequestHandler<GetCurrentEventsQuery, List<string>>
{
    private readonly IEventRepository _repository;
    private readonly IMemoryCache _cache;

    public GetCurrentEventQueryHandler(IEventRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<List<string>> Handle(GetCurrentEventsQuery request, CancellationToken cancellationToken) {
        var currentEvent = await _repository.GetCurrentEventsAsync();
        if (currentEvent == null || !currentEvent.Any())
        {
            throw new Exception("Güncel veri bulunamadı.");
        }
        return currentEvent;
    }
}