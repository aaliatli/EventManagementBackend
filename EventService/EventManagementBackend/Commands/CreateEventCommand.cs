using MediatR;

public class CreateEventCommand : IRequest<Unit>
{
    public string Title { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Capacity { get; set; }
    public Guid CreatorUserId{ get; set; }
    public int AgeRestriction
    { get; set; } = 0;
}