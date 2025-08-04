using MediatR;

public class LoginUserCommand : IRequest<string>
{
    public string Mail { get; set; }
    public string Password { get; set; }
}