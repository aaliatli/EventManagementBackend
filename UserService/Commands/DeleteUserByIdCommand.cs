using MediatR;

public class DeleteUserByIdCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}