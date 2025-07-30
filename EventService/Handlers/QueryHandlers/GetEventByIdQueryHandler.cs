using EventService.Models;
using MediatR;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Event>
{
    private readonly IEventRepository _repository;

    public GetEventByIdQueryHandler(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<Event> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var eventById = await _repository.GetEventById(request.Id);
        if (eventById == null)
        {
            throw new Exception("ID ile getirilecek veri bulunamadı.");
        }
        return eventById;
    }
}