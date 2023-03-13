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
    public void IntercepterBreakTest(int numberOfIntercepter)
    {
        var values = new List<int>();
        var intercepters = Enumerable
            .Range(1, numberOfIntercepter)
            .Select(num => new InterceptDelegate<TheRequest>((ctx, next) => values.Add(num)))
            .ToArray();

        var pipe = new Pipeline<TheRequest>(intercepters);

        var contextMock = new Mock<IRequestContext<TheRequest>>();
#pragma warning disable CS8604 // Possible null reference argument.
        pipe.Run(contextMock.Object);
#pragma warning restore CS8604 // Possible null reference argument.

        Assert.Equal(new[] { 1 }, values);
    }


    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void IntercepterConcatenateTest(int numberOfIntercepter)
    {
        var values = new List<int>();
        var intercepters = Enumerable.Range(1, numberOfIntercepter)
            .Select(num => new InterceptDelegate<TheRequest>((ctx, next) =>
            {
                values.Add(num);
                next(ctx);
            }))
            .ToArray();

        var pipe = new Pipeline<TheRequest>(intercepters);

        var contextMock = new Mock<IRequestContext<TheRequest>>();
#pragma warning disable CS8604 // Possible null reference argument.
        pipe.Run(contextMock.Object);
#pragma warning restore CS8604 // Possible null reference argument.

        Assert.Equal(Enumerable.Range(1, numberOfIntercepter).ToList(), values);
    }
}