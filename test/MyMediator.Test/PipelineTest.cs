using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Dynamic;

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

    [Fact]
    public void OnlyOneMiddlewareNotThrowEmptyPipelineExceptionTest()
    {
        var request = new TheRequest();
        var context = new RequestContext<TheRequest>(request);
        var middlewareMock = new Mock<IRequestMiddleware<TheRequest>>();

        dynamic invoked = new ExpandoObject();
        invoked.Value = false;

        middlewareMock.Setup(middleware => middleware.Handle(request))
            .Callback((IRequest _) => invoked.Value = true);

        var pipe = new Pipeline<TheRequest>(context,
            new IRequestHandler<TheRequest>[0], new[] { middlewareMock.Object });

        pipe.Run();

        Assert.True(invoked.Value);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(1, 2)]
    public void PipelineDefaultTest(int numberOfRequestHandler, int numberOfMiddleware)
    {
        var request = new TheRequest();
        var context = new RequestContext<TheRequest>(request);

        dynamic requestHandlerCounter = new ExpandoObject();
        dynamic middlewareCounter = new ExpandoObject();

        requestHandlerCounter.Counter = 0;
        middlewareCounter.Counter = 0;

        Mock<IRequestHandler<TheRequest>> CreateRequestHandlerMock()
        {
            var requestHandler = new Mock<IRequestHandler<TheRequest>>();
            requestHandler.Setup(handler => handler.Handle(request))
                .Callback(() => requestHandlerCounter.Counter ++);

            return requestHandler;
        }

        Mock<IRequestMiddleware<TheRequest>> CreateRequestMiddlewareMock()
        {
            var middlewareMock = new Mock<IRequestMiddleware<TheRequest>>();
            middlewareMock.Setup(handler => handler.Handle(request))
                .Callback(() => middlewareCounter.Counter ++);

            return middlewareMock;
        }

        var handlers = Enumerable.Range(0, numberOfRequestHandler)
            .Select(_ => CreateRequestHandlerMock().Object)
            .ToArray();
        var middlewares = Enumerable.Range(0, numberOfMiddleware)
            .Select(_ => CreateRequestMiddlewareMock().Object)
            .ToArray();

        var pipe = new Pipeline<TheRequest>(context, handlers, middlewares);

        pipe.Run();

        Assert.Equal(
            (numberOfRequestHandler, numberOfMiddleware),
            (requestHandlerCounter.Counter, middlewareCounter.Counter));
    }
}