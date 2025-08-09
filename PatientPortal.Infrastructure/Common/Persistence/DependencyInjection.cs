using Microsoft.Extensions.DependencyInjection;
using PatientPortal.Persistence.Abstractions.Common.Interfaces;

namespace PatientPortal.Infrastructure.Common.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddDbServices(
        this IServiceCollection self)
    {
        self.AddScoped<IPatientPortalDbContext>(provider => 
            provider.GetRequiredService<PatientPortalDbContext>());

        self.AddHostedService<PatientPortalDbContextMigrationService>();
        
        return self;
    }
}