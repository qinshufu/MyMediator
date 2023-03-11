namespace MyMediator
{
    public interface IRequest
    {
    }



    public interface IRequest<TResponse> where TResponse : IResponse
    {
    }
}