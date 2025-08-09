using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PatientPortal.Infrastructure.Common.Persistence;

public class PatientPortalDbContextFactory 
    : IDesignTimeDbContextFactory<PatientPortalDbContext>
{
    public PatientPortalDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(
                Directory.GetCurrentDirectory(),
                "../PatientPortal.Api"))
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var connectionString = configuration
            .GetSection("Persistence")
            .GetValue<string>("DesignTimeConnectionString");

        var optionsBuilder = 
            new DbContextOptionsBuilder<PatientPortalDbContext>(); 

        optionsBuilder.UseSqlServer(connectionString);

        return new PatientPortalDbContext(optionsBuilder.Options);
    }
}