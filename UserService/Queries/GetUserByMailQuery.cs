using MediatR;
public class GetUserByMailQuery : IRequest<GetUserByMailQueryResult>
{
    public string Mail { get; set; }
}