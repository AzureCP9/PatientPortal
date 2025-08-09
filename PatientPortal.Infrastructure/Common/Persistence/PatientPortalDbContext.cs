using Microsoft.EntityFrameworkCore;
using NodaTime;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Values;
using PatientPortal.Infrastructure.Common.Persistence.ValueConverters;
using PatientPortal.Persistence.Abstractions.Common.Interfaces;
using PatientPortal.Persistence.Abstractions.Consultations;
using PatientPortal.Persistence.Abstractions.Patients;
using System.Reflection;

namespace PatientPortal.Infrastructure.Common.Persistence;

// TODO: add audit timestamps on entities (out of scope)
public class PatientPortalDbContext : DbContext, IPatientPortalDbContext
{
    public DbSet<PatientDbEntity> Patients { get; set; } = default!;
    public DbSet<ConsultationDbEntity> Consultations { get; set; } = default!;

    public PatientPortalDbContext() {}

    public PatientPortalDbContext(
        DbContextOptions<PatientPortalDbContext> options)
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(
        ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<Enum>()
            .HaveConversion<string>();

        configurationBuilder.Properties<Gender>()
            .HaveConversion<GenderValueConverter>();

        configurationBuilder.Properties<Age>()
            .HaveConversion<AgeValueConverter>();

        configurationBuilder.Properties<NonEmptyString>()
            .HaveConversion<NonEmptyStringValueConverter>();

        configurationBuilder.Properties<Instant>()
            .HaveConversion<InstantValueConverter>();

        configurationBuilder.Properties<Duration>()
            .HaveConversion<DurationValueConverter>();

        configurationBuilder.Properties<AbsoluteUri>()
            .HaveConversion<AbsoluteUriValueConverter>();
    }
}