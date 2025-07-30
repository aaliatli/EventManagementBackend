using MediatR;
using Microsoft.Extensions.Caching.Memory;

public class GetAllEventQueryHandler : IRequestHandler<GetAllEventsQuery, List<string>>
{
    private readonly IEventRepository _repository;
    private readonly IMemoryCache _cache;

    public GetAllEventQueryHandler(IEventRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<List<string>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _repository.GetAllEventsAsync();
        if (events == null || !events.Any())
        {
            System.Console.WriteLine("[HANDLER] Veri bulunamadı.");
            throw new Exception("Db' den veri çekme hatası");
        }
        _cache.Remove("GetAllEvents");
        System.Console.WriteLine("[CACHE] Cache temizlendi.");
        
        return events;
    }
 
}