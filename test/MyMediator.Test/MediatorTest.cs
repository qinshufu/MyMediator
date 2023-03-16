using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MyMediator.Test
{
    public class TheRequestWithResposne : IRequest<int>
    {

    }

    class RequestHandler : IRequestHandler<TheRequest>
    {
        private readonly Action<IRequestContext<TheRequest>> _handler;

        public RequestHandler(Action<IRequestContext<TheRequest>> handler)
        {
            _handler = handler;
        }

        public void Handle(IRequestContext<TheRequest> request)
        {
            _handler(request);
        }

    }

    class RequestHandler2 : IRequestHandler<TheRequest>
    {
        private readonly Action<IRequestContext<TheRequest>> _handler;

        public RequestHandler2(Action<IRequestContext<TheRequest>> handler)
        {
            _handler = handler;
        }

        public void Handle(IRequestContext<TheRequest> request)
        {
            _handler(request);
        }
    }

    public class MediatorTest
    {
        public ServiceCollection ServicesSource { get; }

        public MediatorTest()
        {
            ServicesSource = new ServiceCollection();
            ServicesSource.AddMediator();
        }

        [Fact]
        public void ThrowExceptionOnNotHaveRequestHandler()
        {
            ServicesSource.AddMediator();

            var mediator = ServicesSource.BuildServiceProvider().GetRequiredService<IMediator>();

            Assert.Throws<InvalidOperationException>(() => mediator.Send(new TheRequest()));
        }

        [Fact]
        public void SendRequestTest()
        {
            var values = new int[1];

            ServicesSource.AddMediator();
            ServicesSource.AddScoped<IRequestHandler<TheRequest>>(_ => new RequestHandler(_ => values[0] = 1));

            var mediator = ServicesSource.BuildServiceProvider().GetRequiredService<IMediator>();

            mediator.Send(new TheRequest());

            Assert.Equal(new[] { 1 }, values);
        }

        [Fact]
        public void SendRequestAndReturnResponseTest()
        {

            var request = new TheRequestWithResposne();

            IRequestHandler<TheRequestWithResposne> CreateMock()
            {
                var mock = new Mock<IRequestHandler<TheRequestWithResposne>>();
                mock.Setup(m => m.Handle(new RequestContext<TheRequestWithResposne>(request)))
                    .Callback<IRequestContext<TheRequestWithResposne>>(ctx => ctx.Response = 1);

                return mock.Object;
            }

            ServicesSource.AddMediator();
            ServicesSource.AddScoped(_ => CreateMock());


            var mediator = ServicesSource.BuildServiceProvider().GetRequiredService<IMediator>();

            var response = mediator.Send<TheRequestWithResposne, int>(request);

            Assert.Equal(1, response);
        }

        [Fact]
        public void NotifyTestDefault()
        {
            var request = new TheRequest();

            var values = new[] { 1, 2 };

            ServicesSource.AddMediator();
            ServicesSource.AddScoped<IRequestHandler<TheRequest>, RequestHandler>(
                _ => new RequestHandler(_ => values[0] = 0));
            ServicesSource.AddScoped<IRequestHandler<TheRequest>, RequestHandler2>(
                _ => new RequestHandler2(_ => values[1] = 0));

            var mediator = ServicesSource.BuildServiceProvider().GetRequiredService<IMediator>();

            mediator.Send(request);

            Assert.Equal(new[] { 0, 0 }, values);
        }
    }
}
