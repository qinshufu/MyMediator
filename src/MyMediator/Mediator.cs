
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

        public void Notify<TRequest>(TRequest request) where TRequest : IRequest
        {
            using var scope = _serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            // 这里因为 Microsoft 的 scope 实现不能添加服务，所以手动配置
            var context = new RequestContext<TRequest>(request);
            var handlers = provider.GetServices<IRequestHandler<TRequest>>();
            var middlewares = provider.GetServices<IRequestMiddleware<TRequest>>();

            var pipe = new Pipeline<TRequest>(context, handlers.ToArray(), middlewares.ToArray());

            pipe.Run();
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
