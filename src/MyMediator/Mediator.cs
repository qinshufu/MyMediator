
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

        private Pipeline<TRequest> CreatePipeline<TRequest>()
            where TRequest : IRequest
        {
            var scope = _serviceProvider.CreateScope();
            var intercepters = scope.ServiceProvider.GetServices<InterceptDelegate<TRequest>>();
            var handlers = scope.ServiceProvider.GetServices<IRequestHandler<TRequest>>()
                        ?? new IRequestHandler<TRequest>[0];

            if (handlers.Any() is false) // is empty
                throw new InvalidOperationException("没有有效的请求处理器，请检查是否至少注册了一个请求处理器");

            InterceptDelegate<TRequest> requestHandleIntercepter = (ctx, next) =>
            {
                foreach (var handler in handlers)
                {
                    handler.Handle(ctx);
                }

                next(ctx);
            };

            var finalIntercepters = intercepters.Concat(new[] { requestHandleIntercepter }).ToArray();

            var pipe = new Pipeline<TRequest>(finalIntercepters);

            return pipe;
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
