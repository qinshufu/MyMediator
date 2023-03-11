namespace MyMediator
{
    public interface IRequestContext<TRequest> where TRequest : IRequest
    {
        TRequest Request { get; }
    }

    public interface IRequestContext<TRequest, TResponse> : IRequestContext<TRequest>
            where TRequest : IRequest<TResponse>
              where TResponse : IResponse
    {
        TResponse? Response { get; set; }
    }

}
