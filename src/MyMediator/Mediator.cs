
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

        public void Notify(IRequest request)
        {
            var context = new RequestContext(request);
            var pipe = CreatePipeline();

            pipe.Run(context);
        }

        private Pipeline CreatePipeline()
        {
            var scope = _serviceProvider.CreateScope();
            var anyRequestMiddlewares = scope.ServiceProvider.GetServices<IRequestMiddlewareAny>();
            var requestMiddlewares = scope.ServiceProvider.GetServices<IRequestMiddleware>();
            var handlers = scope.ServiceProvider.GetServices<IRequestHandler>();
            var intercepter = CreateIntercepter(anyRequestMiddlewares, requestMiddlewares, handlers);
            var pipe = new Pipeline(new[] { intercepter });

            return pipe;
        }

        private InterceptDelegate CreateIntercepter(
                IEnumerable<IRequestMiddlewareAny> anyRequestMiddlewares,
                IEnumerable<IRequestMiddleware> requestMiddlewares,
                IEnumerable<IRequestHandler> handlers)
        {
            var intercepter = (IRequestContext ctx, RequestDelegate next) =>
            {
                foreach (var mid in new dynamic[0]
                                    .Concat(anyRequestMiddlewares)
                                    .Concat(requestMiddlewares)
                                    .Concat(handlers))
                {
                    mid.Handle(ctx);
                }

                next(ctx);
            };

            return new InterceptDelegate(intercepter);

        }

        public TResponse Notify<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
            where TResponse : IResponse
        {
            var context = new RequestContext(request);
            var pipe = CreatePipeline();
            pipe.Run(context);

            return (TResponse)context.Response!;
        }

        public void Send(IRequest request)
        {
            var context = new RequestContext(request);
            var pipe = CreatePipeline();

            pipe.Run(context);
        }

        public TResponse Send<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
            where TResponse : IResponse
        {
            var context = new RequestContext(request);
            var pipe = CreatePipeline();

            pipe.Run(context);

            return (TResponse)context.Response!;
        }

    }
}
