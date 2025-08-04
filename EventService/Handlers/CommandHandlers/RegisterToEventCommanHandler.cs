using Microsoft.EntityFrameworkCore;
using EventService.Models;
using EventService.Data;
using MediatR;
using AutoMapper;
using System.Net.Http.Json;
using System.Net.Http;


public class RegisterToEventCommandHandler : IRequestHandler<RegisterToEventCommand, string>
{
    private readonly IMediator _mediator;
    private readonly EventDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IEventRepository _repository;

    public RegisterToEventCommandHandler(IMediator mediator, EventDbContext context, IHttpClientFactory httpClientFactory, IEventRepository repository)
    {
        _mediator = mediator;
        _context = context;
        _httpClientFactory = httpClientFactory;
        _repository = repository;
    }

    public async Task<string> Handle(RegisterToEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _mediator.Send(new GetEventByIdQuery { Id = request.EventId });
        if (eventEntity == null) return "Etkinlik bulunamadı.";

        var userEventCount = await _context.UserEvents.CountAsync(ue => ue.EventId == request.EventId);
        if (userEventCount > eventEntity.Capacity) return "Etkinlik dolu.";

        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5020/api/User/{request.UserId}");
        var user = await response.Content.ReadFromJsonAsync<UserDto>();

        if (eventEntity.AgeRestriction > 0 && user.Age < eventEntity.AgeRestriction) return $"Bu etkinliğe katılım yaşı: {eventEntity.AgeRestriction}";


        var userEvent = new UserEvent
        {
            UserId = request.UserId,
            EventId = request.EventId
        };

        _context.UserEvents.Add(userEvent);
        await _context.SaveChangesAsync();
        return "Etkinliğe başarıyla kayıt olundu.";

    }
}