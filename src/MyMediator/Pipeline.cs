namespace MyMediator
{
    public class Pipeline<TRequest> : IPipeline<TRequest>
        where TRequest : IRequest
    {
        private readonly InterceptDelegate<TRequest>[] _intercepters;

        public Pipeline(InterceptDelegate<TRequest>[] requestMiddlewares)
        {
            _intercepters = requestMiddlewares;
        }

        public void Run(IRequestContext<TRequest> context)
        {
            var next = _intercepters[0];
            foreach (var intercepter in _intercepters[1..])
            {
                var prev = next;
                next = (ctx, handler) => prev(ctx, r => intercepter(r, handler));
            }

            next.Invoke(context, new RequestDelegate<TRequest>(Pass));
        }

        private void Pass(IRequestContext<TRequest> context)
        {
            // pass
        }
    }
}
