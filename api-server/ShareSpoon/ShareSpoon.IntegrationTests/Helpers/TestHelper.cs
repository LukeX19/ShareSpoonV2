using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;

namespace ShareSpoon.IntegrationTests.Helpers
{
    public static class TestHelper
    {
        public static IMediator CreateMediator(IUnitOfWork unitOfWork)
        {
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IRecipeRepository).Assembly));
            services.AddSingleton(unitOfWork);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddLogging(cfg => cfg.AddConsole())
                    .AddTransient(typeof(ILogger<>), typeof(Logger<>));

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMediator>();
        }
    }
}
