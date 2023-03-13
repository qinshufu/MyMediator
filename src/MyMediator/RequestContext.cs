namespace MyMediator
{
    public class RequestContext : IRequestContext
    {
        private readonly IRequest _request;

        public RequestContext(IRequest request)
        {
            _request = request;
        }

        public IResponse? Response { get; set; }
        public IRequest Request => _request;
    }
}
