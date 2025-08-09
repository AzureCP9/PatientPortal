using Microsoft.Extensions.Configuration;

namespace PatientPortal.AppHost.Config.SqlServer;

public static class SqlServerConfigurator
{
    public static IResourceBuilder<SqlServerDatabaseResource> ConfigureSqlServer(
        this IDistributedApplicationBuilder self)
    {
        var config = self.Configuration.GetSection("Persistence:SqlServer");
        var port = config.GetValue<int>("Port");
        var password = config.GetValue<string>("Password")
            ?? throw new ArgumentNullException("Missing Sql Server password.");

        var passwordParameter = self.AddParameter(
            "SqlServerPassword", password);

        var sqlServer = self
            .AddSqlServer(
                name: "PatientPortalSqlServer",
                port: port,
                password: passwordParameter)
            .WithLifetime(ContainerLifetime.Persistent)
            .AddDatabase("PatientPortalDb");

        return sqlServer;
    }
}
