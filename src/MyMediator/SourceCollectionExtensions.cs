using Microsoft.Extensions.DependencyInjection;

namespace MyMediator
{
    public static class SourceCollectionExtensions
    {
        public static void AddMediator(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, Mediator>();
        }
    }
}
