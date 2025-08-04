using EventService.Data;
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
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var result = await _mediator.Send(new SearchEventQuery(keyword));
        return Ok(result);
    }

    [Authorize]
    [HttpPost("register-to-event")]
    public async Task<IActionResult> RegisterToEvent(Guid eventId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _httpClientFactory.CreateClient().GetAsync($"http://localhost:5016/api/User/{userId}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound("Kullanıcı bulunamadı");
        }

        var user = await response.Content.ReadFromJsonAsync<UserDto>();

        var result = await _mediator.Send(new RegisterToEventCommand
        {
            EventId = eventId,
            UserId = userId,
        });

        return Ok(result);
    }

    [HttpPost("export-excel")]
    public async Task<IActionResult> ExportExcel()
    {
        var filePath = await _mediator.Send(new ExportEventsToExcelCommand());
        var fileName = Path.GetFileName(filePath);
        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

}