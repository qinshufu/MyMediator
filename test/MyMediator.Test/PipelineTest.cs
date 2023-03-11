using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MyMediator.Test;

public class TheRequest : IRequest
{
    public int Counter = 0;
}

public class PipelineTest
{
    private readonly ServiceCollection _services;

    public PipelineTest()
    {
        _services = new ServiceCollection();
        _services.AddMediator();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void OnlyOneRequestHandlerSuccessTest(int count)
    {
        var request = new TheRequest();
        var context = new RequestContext<TheRequest>(request);
        var handlers = Enumerable.Range(0, count).Select(_ =>
        {
            var handlerMock = new Mock<IRequestHandler<TheRequest>>();
            handlerMock.Setup(handler => handler.Handle(request)).Callback(() => request.Counter++);

            return handlerMock.Object;
        }).ToArray();


        var pipe = new Pipeline<TheRequest>(context, handlers, new IRequestMiddleware<TheRequest>[0]);

        pipe.Run();

        Assert.Equal(count, request.Counter);
    }

    [Fact]
    public void EmptyRequestPipelineExceptionRaiseTest()
    {
        var request = new TheRequest();
        var context = new RequestContext<TheRequest>(request);

        var pipe = new Pipeline<TheRequest>(context, new IRequestHandler<TheRequest>[0], new IRequestMiddleware<TheRequest>[0]);

        // 空的请求管道
        Assert.Throws<InvalidOperationException>(() => pipe.Run());
    }
}