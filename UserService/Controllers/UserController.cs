using UserService.Data;
using UserService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery { Id = id });
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet("by-mail")]
    public async Task<IActionResult> GetUserByMail(string mail)
    {
        var user = await _mediator.Send(new GetUserByIdQuery());
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> UserRegister(RegisterUserCommand command)
    {
        await _mediator.Send(command);
        return Ok("Kullanıcı kayıt edildi.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        var token = await _mediator.Send(command);
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("Mail veya şifre hatalı.");
        }
        return Ok(new { token });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var currentUserId))
        {
            return Unauthorized("Kullanıcı bulunamadı.");
        }

        var command = new DeleteUserByIdCommand
        {
            Id = id
        };

        if (currentUserId != id)
        {
            return Forbid("Yalnızca kendi hesabınızı silebilirsiniz!");
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return BadRequest("Silme işlemi başarısız oldu.");
        }
        return Ok("Kullanıcı silindi");
    }

}