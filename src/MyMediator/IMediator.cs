namespace MyMediator;

public interface IMediator
{
    void Send<TRequest>(TRequest request) where TRequest : IRequest;


    TResponse Send<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse;


    void Notify<TRequest>(TRequest request) where TRequest : IRequest;


    TResponse Notify<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse;
}
