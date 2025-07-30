using EventService.Models;
using MediatR;

public class DeleteEventCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid RequestingUserId { get; set; }
}