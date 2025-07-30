using System.Diagnostics;
using MediatR;
using Serilog;

public class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        Log.Information("[LOG] Loglama başladı.", requestName);
        var response = await next();
        Log.Information("[LOG] loglama bitti", requestName);
        return response;
    }
}

