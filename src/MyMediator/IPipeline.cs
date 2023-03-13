namespace MyMediator
{
    public interface IPipeline
    {
        void Run(IRequestContext context);
    }
}
