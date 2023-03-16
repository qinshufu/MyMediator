namespace MyMediator
{
    public delegate void RequestDelegate<TRequest>(IRequestContext<TRequest> request)
        where TRequest : IRequest;

    public delegate void InterceptDelegate<TRequest>(
        IRequestContext<TRequest> request, RequestDelegate<TRequest> next)
        where TRequest : IRequest;

    public interface IRequestMiddlewareAny<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
    }

    public interface IRequestMiddleware<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
    }
}
