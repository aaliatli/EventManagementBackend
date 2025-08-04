using EventService.Models;
using MediatR;

public class GetCurrentEventsQuery : IRequest<List<GetCurrentEventsQueryResult>>
{
}