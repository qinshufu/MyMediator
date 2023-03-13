namespace MyMediator
{
    public interface IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        void Handle(IRequestContext<TRequest> request);
    }

}