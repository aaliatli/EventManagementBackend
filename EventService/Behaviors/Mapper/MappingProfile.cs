using EventService.Models;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Event, GetAllEventsQueryResult>();
        CreateMap<Event, GetCurrentEventsQueryResult>();
        CreateMap<Event, GetEventByIdQueryResult>();
    }
}