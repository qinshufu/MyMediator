
namespace MyMediator
{
    internal class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Notify<TRequest>(TRequest request) where TRequest : IRequest
        {
            throw new NotImplementedException();
        }

        public TResponse Notify<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
            where TResponse : IResponse
        {
            throw new NotImplementedException();
        }

        public void Send<TRequest>(TRequest request) where TRequest : IRequest
        {
            throw new NotImplementedException();
        }

        public TResponse Send<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
            where TResponse : IResponse
        {
            throw new NotImplementedException();
        }
    }
}
