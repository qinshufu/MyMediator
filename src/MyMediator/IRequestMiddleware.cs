namespace MyMediator
{
    public delegate void RequestDelegate<TRequest>(TRequest request) where TRequest : IRequest;

    public delegate void InterceptDelegate<TRequest>(TRequest request, RequestDelegate<TRequest> next) where TRequest : IRequest;

    public interface IRequestMiddleware<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        void Handle(RequestDelegate<TRequest> next);
    }
}
