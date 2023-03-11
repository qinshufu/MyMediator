namespace MyMediator
{
    internal class Pipeline<TRequest> : IPipeline<TRequest>
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
            // TODO 处理需要响应的请求
            InterceptDelegate<TRequest> pipe = (request, next) => next(request);

            foreach (var middleware in _middleware)
            {
                var last = pipe;
                pipe = (request, next) => last(request, next);
            }

            pipe.Invoke(_requestContext.Request, ThrowEmptyPipelineException);
        }

        private void Handle(TRequest request)
        {
            foreach (var handler in _handlers)
                handler.Handle(_requestContext.Request);
        }

        private void ThrowEmptyPipelineException(TRequest request)
        {
            throw new InvalidOperationException("空的请求管道");
        }
    }
}
