using Aspire.Hosting.Azure;

namespace PatientPortal.AppHost.Config.AzureStorage;

public static class AzureStorageConfigurator
{
    public static IResourceBuilder<AzureBlobStorageResource> ConfigureAzureStorage(
        this IDistributedApplicationBuilder self)
    {
        var azureStorage = self
            .AddAzureStorage("azurestorage")
            .RunAsEmulator(x => x.WithLifetime(ContainerLifetime.Persistent));

        var storageIdentifier = "PatientPortalAzureBlob";
        return azureStorage.AddBlobs(storageIdentifier);
    }
}