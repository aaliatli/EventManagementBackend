using UserService.Data;
using UserService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
public class GetUserByMailQueryHandler : IRequestHandler<GetUserByMailQuery, GetUserByMailQueryResult>
{
    private readonly IUserRepository _repository;

    public GetUserByMailQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetUserByMailQueryResult> Handle(GetUserByMailQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByMailAsync(request.Mail);
        if (user == null) return null;
        var userResponse = new GetUserByMailQueryResult
        {
            Mail = user.Mail
        };
        return userResponse;
    }
}