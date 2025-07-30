using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;

public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, bool>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<DeleteUserByIdCommandHandler> _logger;
    public DeleteUserByIdCommandHandler(IUserRepository repository, ILogger<DeleteUserByIdCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserById(request.Id);
        if (user.IsDeleted)
        {
            _logger.LogInformation("[LOG] User silindi");
            return false;
        }
        await _repository.DeleteUserById(request.Id);
        return true;

    }
}