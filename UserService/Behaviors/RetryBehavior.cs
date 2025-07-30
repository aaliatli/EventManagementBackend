using MediatR;
using Polly;


public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(500)
        ,onRetry: (exception, timespan, retryCount, context) =>
        {
            Console.WriteLine($"[RETRY] Deneme: {retryCount}, Hata: {exception.Message}");
        });

        return await retryPolicy.ExecuteAsync(() => next());
    }
}