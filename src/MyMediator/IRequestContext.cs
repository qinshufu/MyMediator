namespace MyMediator
{
    public interface IRequestContext
    {
        IRequest Request { get; }

        IResponse? Response { get; set; }
    }

}
