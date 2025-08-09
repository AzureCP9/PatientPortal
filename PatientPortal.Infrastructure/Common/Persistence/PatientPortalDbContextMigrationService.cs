using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PatientPortal.Infrastructure.Common.Persistence;

public class PatientPortalDbContextMigrationService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public PatientPortalDbContextMigrationService(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = 
            scope.ServiceProvider.GetRequiredService<PatientPortalDbContext>();

        await db.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => 
        Task.CompletedTask;
}