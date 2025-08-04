using UserService.Models;
using MediatR;

namespace UserService.Models
{
    public class GetUserByIdQuery : IRequest<GetUserByIdQueryResult>
    {
        public Guid Id { get; set; }
    } 
}
