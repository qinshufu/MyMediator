namespace MyMediator
{
    public interface IRequestHandler
    {
        void Handle(IRequestContext request);
    }

}