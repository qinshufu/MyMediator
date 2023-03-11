namespace MyMediator
{
    public interface IPipeline<in TRequest>
        where TRequest : IRequest
    {
        void Run();
    }

    public interface IPipeline<in TRequest, in TResponse> : IPipeline<TRequest>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
    }
}
