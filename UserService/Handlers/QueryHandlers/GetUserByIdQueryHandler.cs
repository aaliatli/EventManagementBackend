using UserService.Data;
using UserService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResult>
{
    private readonly IUserRepository _repository;

    public GetUserByIdQueryHandler(IUserRepository repository){
        _repository = repository;
    }

    public async Task<GetUserByIdQueryResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken){
        var user = await _repository.GetUserById(request.Id);
        var userResponse = new GetUserByIdQueryResult
        {
            Id = user.Id,
        };
        return userResponse;
    }

}