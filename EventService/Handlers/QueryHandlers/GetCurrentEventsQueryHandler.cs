using MediatR;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;

public class GetCurrentEventsQueryHandler : IRequestHandler<GetCurrentEventsQuery, List<GetCurrentEventsQueryResult>>
{
    private readonly IEventRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetCurrentEventsQueryHandler(IEventRepository repository, IMemoryCache cache, IMapper mapper)
    {
        _repository = repository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<List<GetCurrentEventsQueryResult>> Handle(GetCurrentEventsQuery request, CancellationToken cancellationToken) {
        var currentEvent = await _repository.GetCurrentEventsAsync();
        if (currentEvent == null || !currentEvent.Any())
        {
            throw new Exception("Güncel veri bulunamadı.");
        }
        var result = _mapper.Map<List<GetCurrentEventsQueryResult>>(currentEvent);
        return result;
    }
}