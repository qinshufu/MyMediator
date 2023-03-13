namespace MyMediator
{
    public delegate void RequestDelegate<TRequest>(IRequestContext<TRequest> request)
        where TRequest : IRequest;

    public delegate void InterceptDelegate<TRequest>(
        IRequestContext<TRequest> request, RequestDelegate<TRequest> next)
        where TRequest : IRequest;

    public interface IRequestMiddlewareAny<TRequest>
        where TRequest : IRequest
    {
        void Handle(IRequestContext<TRequest> request);
    }

    public interface IRequestMiddleware<TRequest>
        where TRequest : IRequest
    {
        void Handle(IRequestContext<TRequest> request);
    }
}
