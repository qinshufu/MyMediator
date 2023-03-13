namespace MyMediator
{
    public record class RequestContext<TRequest> : IRequestContext<TRequest>
        where TRequest : IRequest
    {
        private readonly TRequest _request;

        public RequestContext(TRequest request)
        {
            _request = request;
        }

        public object? Response { get; set; }

        public TRequest Request => _request;

    }
}
