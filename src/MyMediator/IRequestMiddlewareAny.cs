namespace MyMediator
{
    public delegate void RequestDelegate(IRequestContext request);

    public delegate void InterceptDelegate(IRequestContext request, RequestDelegate next);

    public interface IRequestMiddlewareAny
    {
        void Handle(IRequestContext request);
    }

    public interface IRequestMiddleware
    {
        void Handle(IRequestContext request);
    }
}
