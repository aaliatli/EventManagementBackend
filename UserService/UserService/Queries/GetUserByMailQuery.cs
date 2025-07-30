using MediatR;
public class GetUserByMailQuery : IRequest<User>
{
    public string Mail { get; set; }
}