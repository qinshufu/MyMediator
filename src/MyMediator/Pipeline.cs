namespace MyMediator
{
    internal class Pipeline<TRequest, TRequestHandler>
        where TRequestHandler : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        private readonly TRequestHandler[] _handlers;
        private readonly TRequest _request;

        public Pipeline(TRequest request, params TRequestHandler[] handlers)
        {
            _handlers = handlers;
            _request = request;
        }

        public void Start()
        {
            foreach (var handler in _handlers)
                handler.Handle(_request);
        }
    }
}
