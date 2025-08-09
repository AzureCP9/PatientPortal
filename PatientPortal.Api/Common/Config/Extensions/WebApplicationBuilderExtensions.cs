using Microsoft.AspNetCore.Http.Json;
using PatientPortal.Api.Common.Config.Json;
using PatientPortal.Application;
using PatientPortal.Infrastructure;
using PatientPortal.Infrastructure.Common.Persistence;

namespace PatientPortal.Api.Common.Config.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureWebApplication(this WebApplicationBuilder self)
    {
        //self.ConfigureAzureStorage();

        self.ConfigurePersistence();
        self.ConfigureStorage();

        self.Services.AddInfrastructureServices();
        self.Services.AddApplicationServices();

        self.Services.AddEndpointsApiExplorer();
        self.Services.AddSwaggerGen();

        self.ConfigureJsonOptions();
        self.ConfigureCors();
    }

    private static WebApplicationBuilder ConfigurePersistence(
        this WebApplicationBuilder self)
    {
        // not bothering with non local dev setup for the scope of this project
        self.AddSqlServerDbContext<PatientPortalDbContext>("PatientPortalDb");
        return self;
    }

    public static WebApplicationBuilder ConfigureStorage(
        this WebApplicationBuilder self)
    {
        self.AddAzureBlobServiceClient("PatientPortalAzureBlob");
        return self;
    }

    private static WebApplicationBuilder ConfigureJsonOptions(
        this WebApplicationBuilder self)
    {
        self.Services.Configure<JsonOptions>(options =>
        {
            var globalJsonOptions = GlobalJsonOptions.WebJsonOptions;

            foreach (var converter in globalJsonOptions.Converters)
                options.SerializerOptions.Converters.Add(converter);
        });

        return self;
    }

    private static WebApplicationBuilder ConfigureCors(
        this WebApplicationBuilder self)
    {
        self.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy
                    // hardcode a simple policy for demo simplicity
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );
        });

        return self;
    }
}
