using EventService.Models;
using MediatR;

public class GetAllEventsPagedQuery : IRequest<PagedResponse<Event>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}