using System;

namespace EventService.Models;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Capacity { get; set; }
    public int AgeRestriction {get; set;}
    public Guid CreatorUserId{ get; set; }
    public bool IsDeleted { get; set; } = false;
    public string SearchText => $"{Title}{Location}{StartDate:dd-MM-yyyy}{EndDate:dd-MM-yyyy}{Capacity}";
    public ICollection <UserEvent> UserEvents { get; set; } = new List<UserEvent>();
}
