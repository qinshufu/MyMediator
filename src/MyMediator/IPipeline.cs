namespace MyMediator
{
    public delegate void RequestDelegate<TRequest>(IRequestContext<TRequest> request)
    where TRequest : IRequest;

    public delegate void InterceptDelegate<TRequest>(
        IRequestContext<TRequest> request, RequestDelegate<TRequest> next)
        where TRequest : IRequest;

    public interface IPipeline<TRequest>
        where TRequest : IRequest
    {
        void Run(IRequestContext<TRequest> context);
    }
}
