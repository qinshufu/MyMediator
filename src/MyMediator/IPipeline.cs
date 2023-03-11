namespace MyMediator
{
    public interface IPipeline<TRequest>
        where TRequest : IRequest
    {
        void Run();
    }
}
