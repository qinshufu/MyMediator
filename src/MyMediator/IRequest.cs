namespace MyMediator
{
    public interface IRequest
    {
    }



    public interface IRequest<TResponse> : IRequest 
    {
    }
}