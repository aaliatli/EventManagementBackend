using MediatR;
using Microsoft.Extensions.Caching.Memory;
using EventService.Models;
using AutoMapper;


public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, List<GetAllEventsQueryResult>>
{
    private readonly IEventRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetAllEventsQueryHandler(IEventRepository repository, IMemoryCache cache, IMapper mapper)
    {
        _repository = repository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<List<GetAllEventsQueryResult>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _repository.GetAllEventsAsync();
        if (events == null || !events.Any())
        {
            System.Console.WriteLine("[HANDLER] Veri bulunamadı.");
            throw new Exception("Db' den veri çekme hatası");
        }
        var result = _mapper.Map<List<GetAllEventsQueryResult>>(events);

        _cache.Remove("GetAllEvents");
        System.Console.WriteLine("[CACHE] Cache temizlendi.");
        
        return result;
    }
 
}