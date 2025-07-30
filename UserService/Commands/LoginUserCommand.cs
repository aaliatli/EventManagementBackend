using MediatR;

public class LoginUserCommand : IRequest<string>
{
    public Guid Id { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
}