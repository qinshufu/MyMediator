namespace MyMediator
{
    public interface IPipeline<TRequest>
        where TRequest : IRequest
    {
        void Run();
    }

    public interface IPipeline<TRequest, TResponse> : IPipeline<TRequest>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        void Run();
    }
}
