namespace MyMediator
{
    public class Pipeline<TRequest> : IPipeline<TRequest>
        where TRequest : IRequest
    {
        private readonly IRequestHandler<TRequest>[] _handlers;
        private readonly IRequestContext<TRequest> _requestContext;
        private readonly IRequestMiddleware<TRequest>[] _middleware;

        public Pipeline(IRequestContext<TRequest> context, IRequestHandler<TRequest>[] requestHandlers, IRequestMiddleware<TRequest>[] requestMiddlewares)
        {
            _requestContext = context;
            _handlers = requestHandlers;
            _middleware = requestMiddlewares;
        }

        public void Run()
        {
            var pipe = Handle;

            foreach (var middleware in _middleware)
            {
                var last = pipe;
                pipe = (request, next) => last(request, next);
            }

            pipe.Invoke(_requestContext.Request, ThrowEmptyPipelineException);
        }

        private void Handle(TRequest request, RequestDelegate<TRequest> next)
        {
            foreach (var handler in _handlers)
                handler.Handle(request);

            next(request);
        }

        private void ThrowEmptyPipelineException(TRequest request)
        {
            throw new InvalidOperationException("空的请求管道");
        }
    }

    public class Pipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        private readonly IRequestContext<TRequest, TResponse> _requestContext;
        private readonly IRequestHandler<TRequest, TResponse>[] _handlers;
        private readonly IRequestMiddleware<TRequest>[] _middleware;

        public Pipeline(
            IRequestContext<TRequest, TResponse> context,
            IRequestHandler<TRequest, TResponse>[] requestHandlers,
            IRequestMiddleware<TRequest>[] requestMiddlewares)
        {
            _requestContext = context;
            _handlers = requestHandlers;
            _middleware = requestMiddlewares;
        }

        public void Run()
        {
            var pipe = Handle;

            foreach (var middleware in _middleware)
            {
                var last = pipe;
                pipe = (request, next) => last(request, next);
            }

            pipe.Invoke(_requestContext.Request, ThrowEmptyPipelineException);
        }

        private void Handle(TRequest request, RequestDelegate<TRequest> next)
        {
            foreach (var handler in _handlers)
                _requestContext.Response = handler.Handle(request);

            next(request);
        }

        private void ThrowEmptyPipelineException(TRequest request)
        {
            throw new InvalidOperationException("空的请求管道");
        }
    }
}
