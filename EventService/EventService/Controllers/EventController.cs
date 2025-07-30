using EventService.Data;
using EventService.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security.Claims;


[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IMediator _mediator;
    private const int Capacity = 50;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly EventDbContext _context;

    public EventController(IMediator mediator, IHttpClientFactory httpClientFactory, EventDbContext context)
    {
        _mediator = mediator;
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateEventCommand command)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Geçersiz kullanıcı kimliği.");
        }

        command.CreatorUserId = userId;

        await _mediator.Send(command);
        return Ok("Etkinlik başarıyla oluşturuldu.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
       
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Kullanıcı bulunamadı");
        }

        var command = new DeleteEventCommand
        {
            EventId = id,
            RequestingUserId = userId
        };

        var result = await _mediator.Send(command);
        if (!result)
        {
            return Forbid("Bu etkinliği silmeye yetkiniz yok!");
        }
        return Ok("Etkinlik silindi.");
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllEventsQuery()));
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent()
    {
        return Ok(await _mediator.Send(new GetCurrentEventsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _mediator.Send(new GetEventByIdQuery { Id = id }));
    }
    
    [HttpGet("paged")]
    public async Task<IActionResult> GetAllPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllEventsPagedQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword){
        var result = await _mediator.Send(new SearchEventQuery(keyword));
        return Ok(result);
    }

    [Authorize]
    [HttpPost("register-to-event")]
    public async Task<IActionResult> RegisterToEvent(Guid eventId){
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _httpClientFactory.CreateClient().GetAsync($"http://localhost:5016/api/User/{userId}");

        if(!response.IsSuccessStatusCode){
            return NotFound("Kullanıcı bulunamadı");
        }

        var user = await response.Content.ReadFromJsonAsync<UserDto>();

        var eventEntity = await _mediator.Send(new GetEventByIdQuery {Id = eventId});
        if(eventEntity == null){
            return NotFound("Etkinlik bulunamadı.");
        }

        var userEventCount = await _context.UserEvents.CountAsync(ue => ue.EventId == eventId);
        if(userEventCount > eventEntity.Capacity){
            return BadRequest("Etkinlik kapasitesi dolmuş.");
        }

        if(eventEntity.AgeRestriction > 0 && user.Age< eventEntity.AgeRestriction){
            return BadRequest($"Bu etkinlik için minimum yaş sınırı: {eventEntity.AgeRestriction}");
        }

        var userEvent = new UserEvent{
            UserId = userId, 
            EventId = eventId
        };

        _context.UserEvents.Add(userEvent);
        await _context.SaveChangesAsync();

        return Ok("Etkinliğe kayıt olundu. ");
    }

}