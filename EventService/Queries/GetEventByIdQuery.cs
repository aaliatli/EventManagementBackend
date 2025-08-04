using EventService.Models;
using MediatR;

public class GetEventByIdQuery : IRequest<GetEventByIdQueryResult>
{
    public Guid Id{ get; set; }
}