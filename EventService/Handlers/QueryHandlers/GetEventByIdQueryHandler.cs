using EventService.Models;
using MediatR;
using AutoMapper;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, GetEventByIdQueryResult>
{
    private readonly IEventRepository _repository;
    private readonly IMapper _mapper;

    public GetEventByIdQueryHandler(IEventRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetEventByIdQueryResult> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var eventById = await _repository.GetEventById(request.Id);
        if (eventById == null)
        {
            throw new Exception("ID ile getirilecek veri bulunamadı.");
        }
        var result = _mapper.Map<GetEventByIdQueryResult>(eventById);
        return result;
    }
}