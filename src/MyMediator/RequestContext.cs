namespace MyMediator
{
    public class RequestContext<TRequest> : IRequestContext<TRequest> where TRequest : IRequest
    {
        private readonly TRequest _request;

        public RequestContext(TRequest request)
        {
            _request = request;
        }

        public TRequest Request => _request;
    }


    public class RequestContext<TRequest, TResponse> : IRequestContext<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        private readonly TRequest _request;

        public RequestContext(TRequest request)
        {
            _request = request;
        }

        public TResponse? Response { get; set; }

        public TRequest Request => _request;
    }
}
