using UserService.Data;
using UserService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
public class GetUserByMailQueryHandler : IRequestHandler<GetUserByMailQuery, User>
{
    private readonly IUserRepository _repository;

    public GetUserByMailQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> Handle(GetUserByMailQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetUserByMailAsync(request.Mail);
    }
}