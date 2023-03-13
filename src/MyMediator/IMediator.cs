namespace MyMediator;

public interface IMediator
{
    void Send(IRequest request);


    TResponse Send<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse;


    void Notify(IRequest request);


    TResponse Notify<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse;
}
