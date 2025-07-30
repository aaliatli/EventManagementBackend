using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _repository;
    private readonly JwtService _jwtService;

    public LoginUserCommandHandler(IUserRepository repository, JwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.LoginUser(request.Mail, request.Password);

        if (user == null)
        {
            Log.Warning("[LOG] başarısız login denemesi. Mail: {Mail}", request.Mail);
        }

        var token = _jwtService.GenerateToken(user.Id.ToString(), "User");
        return token;
    }
}