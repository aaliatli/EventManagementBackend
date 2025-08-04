using EventService.Models;
using MediatR;

public class GetAllEventsQuery : IRequest<List<GetAllEventsQueryResult>>, ICacheable
{
    public string CacheKey => "GetAllEvents";
    public int CacheDuration => 10;
}