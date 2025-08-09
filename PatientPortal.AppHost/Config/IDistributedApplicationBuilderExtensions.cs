using PatientPortal.AppHost.Config.AzureStorage;
using PatientPortal.AppHost.Config.SqlServer;

namespace PatientPortal.AppHost.Config;

public static class IDistributedApplicationBuilderExtensions
{
    public static IDistributedApplicationBuilder ConfigureProject(
        this IDistributedApplicationBuilder self)
    {
        var sqlServer = self.ConfigureSqlServer();
        var blobStorage = self.ConfigureAzureStorage();

        var project = self.AddProject<Projects.PatientPortal_Api>(
            "patientportal-api");

        project
            .WithReference(sqlServer)
            .WaitFor(sqlServer, WaitBehavior.WaitOnResourceUnavailable)
            .WithReference(blobStorage)
            .WaitFor(blobStorage, WaitBehavior.WaitOnResourceUnavailable);

        return self;
    }
}
