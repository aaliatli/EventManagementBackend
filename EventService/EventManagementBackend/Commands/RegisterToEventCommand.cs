using MediatR;

public class RegisterToEventCommand : IRequest<string>
{
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
}
