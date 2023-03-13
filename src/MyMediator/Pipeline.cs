namespace MyMediator
{
    public class Pipeline : IPipeline
    {
        private readonly InterceptDelegate[] _intercepters;

        public Pipeline(InterceptDelegate[] requestMiddlewares)
        {
            _intercepters = requestMiddlewares;
        }

        public void Run(IRequestContext context)
        {
            var next = _intercepters[0];
            foreach (var intercepter in _intercepters[1..])
            {
                var prev = next;
                next = (ctx, handler) => prev(ctx, r => intercepter(r, handler));
            }

            next.Invoke(context, new RequestDelegate(Pass));
        }

        private void Pass(IRequestContext context)
        {
            // pass
        }
    }
}
