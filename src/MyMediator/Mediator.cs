
using Microsoft.Extensions.DependencyInjection;

namespace MyMediator
{
    internal class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Notify<TRequest>(TRequest request)
            where TRequest : IRequest
        {
            var context = new RequestContext<TRequest>(request);
            var pipe = CreatePipeline<TRequest>();

            pipe.Run(context);
        }

        private Pipeline<TRequest> CreatePipeline<TRequest>()
            where TRequest : IRequest
        {
            var scope = _serviceProvider.CreateScope();
            var anyRequestMiddlewares = scope.ServiceProvider.GetServices<IRequestMiddlewareAny<TRequest>>()
                        ?? new IRequestMiddlewareAny<TRequest>[0];
            var requestMiddlewares = scope.ServiceProvider.GetServices<IRequestMiddleware<TRequest>>()
                        ?? new IRequestMiddleware<TRequest>[0];
            var handlers = scope.ServiceProvider.GetServices<IRequestHandler<TRequest>>()
                        ?? new IRequestHandler<TRequest>[0];

            if (handlers.Any() is false) // is empty
                throw new InvalidOperationException("没有有效的请求处理器，请检查是否至少注册了一个请求处理器");

            var intercepter = CreateIntercepter(
                anyRequestMiddlewares,
                requestMiddlewares,
                handlers);
            var pipe = new Pipeline<TRequest>(new[] { intercepter });

            return pipe;
        }

        private InterceptDelegate<TRequest> CreateIntercepter<TRequest>(
                IEnumerable<IRequestMiddlewareAny<TRequest>> anyRequestMiddlewares,
                IEnumerable<IRequestMiddleware<TRequest>> requestMiddlewares,
                IEnumerable<IRequestHandler<TRequest>> handlers)
            where TRequest : IRequest
        {
            var intercepter = (IRequestContext<TRequest> ctx, RequestDelegate<TRequest> next) =>
            {
                foreach (var mid in new IRequestHandler<TRequest>[0]
                                    .Concat(anyRequestMiddlewares)
                                    .Concat(requestMiddlewares)
                                    .Concat(handlers))
                {
                    mid.Handle(ctx);
                }

                next(ctx);
            };

            return new InterceptDelegate<TRequest>(intercepter);

        }

        public TResponse Notify<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            var context = new RequestContext<TRequest>(request);
            var pipe = CreatePipeline<TRequest>();
            pipe.Run(context);

            return (TResponse)context.Response!;
        }

        public void Send<TRequest>(TRequest request)
            where TRequest : IRequest
        {
            var context = new RequestContext<TRequest>(request);
            var pipe = CreatePipeline<TRequest>();

            pipe.Run(context);
        }

        public TResponse Send<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            var context = new RequestContext<TRequest>(request);
            var pipe = CreatePipeline<TRequest>();

            pipe.Run(context);

            return (TResponse)context.Response!;
        }

    }
}
