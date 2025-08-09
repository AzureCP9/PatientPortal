using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using PatientPortal.Infrastructure.Common.Persistence;

namespace PatientPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection self) =>
            self.AddSingleton<IClock>(SystemClock.Instance)
                .AddDbServices();
}