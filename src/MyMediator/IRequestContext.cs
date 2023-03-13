namespace MyMediator
{
    public interface IRequestContext<TRequest>
        where TRequest : IRequest
    {
        TRequest Request { get; }

        object? Response { get; set; }
    }

}
