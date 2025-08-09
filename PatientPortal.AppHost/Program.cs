using PatientPortal.AppHost.Config;

var builder = DistributedApplication.CreateBuilder(args);

builder.ConfigureProject();

builder.Build().Run();
