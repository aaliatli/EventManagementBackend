using EventService.Models;
using MediatR;

public class SearchEventQueryHandler : IRequestHandler<SearchEventQuery, List<Event>>
{
    private readonly IEventRepository _repository;

    public SearchEventQueryHandler(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Event>> Handle(SearchEventQuery request, CancellationToken cancellationToken){
        var allEvents = await _repository.GetSearchedEvents();
        return allEvents.Where(e => e.SearchText.ToLower().Contains(request.Keyword.ToLower())).ToList();
    }
}