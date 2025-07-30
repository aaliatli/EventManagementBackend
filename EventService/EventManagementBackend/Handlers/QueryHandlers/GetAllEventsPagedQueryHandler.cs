using EventService.Data;
using EventService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;

public class GetAllEventsPagedQueryHandler : IRequestHandler<GetAllEventsPagedQuery, PagedResponse<Event>>
{
    private readonly EventDbContext _context;

    public GetAllEventsPagedQueryHandler(EventDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<Event>> Handle(GetAllEventsPagedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Events.AsQueryable();

        var totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<Event>
        {
            Data = data,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = totalRecords
        };
    }
}