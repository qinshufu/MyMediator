
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

        public void Notify<TRequest>(TRequest request) where TRequest : IRequest => CreatePipeline(new RequestContext<TRequest>(request)).Run();

        public TResponse Notify<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
            where TResponse : IResponse
        {
            var context = new RequestContext<TRequest, TResponse>(request);
            var pipe = CreatePipeline(context);
            pipe.Run();

            return context.Response ?? throw new NullReferenceException("响应为 null");
        }

        public void Send<TRequest>(TRequest request) where TRequest : IRequest => CreatePipeline(new RequestContext<TRequest>(request)).Run();

        public TResponse Send<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
            where TResponse : IResponse
        {
            var context = new RequestContext<TRequest, TResponse>(request);
            var pipe = CreatePipeline(context);
            pipe.Run();

            return context.Response ?? throw new NullReferenceException("响应为 null");
        }


        private Pipeline<TRequest, TResponse> CreatePipeline<TRequest, TResponse>(IRequestContext<TRequest, TResponse> context)
            where TRequest : IRequest<TResponse>
            where TResponse : IResponse
        {
            using var scope = _serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            // 这里因为 Microsoft 的 scope 实现不能添加服务，所以手动配置
            var handlers = provider.GetServices<IRequestHandler<TRequest, TResponse>>();
            var middlewares = provider.GetServices<IRequestMiddleware<TRequest>>();

            var pipe = new Pipeline<TRequest, TResponse>(context, handlers.ToArray(), middlewares.ToArray());

            return pipe;
        }

        private Pipeline<TRequest> CreatePipeline<TRequest>(IRequestContext<TRequest> context)
            where TRequest : IRequest
        {
            using var scope = _serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            // 这里因为 Microsoft 的 scope 实现不能添加服务，所以手动配置
            var handlers = provider.GetServices<IRequestHandler<TRequest>>();
            var middlewares = provider.GetServices<IRequestMiddleware<TRequest>>();

            var pipe = new Pipeline<TRequest>(context, handlers.ToArray(), middlewares.ToArray());

            return pipe;
        }
    }
}
