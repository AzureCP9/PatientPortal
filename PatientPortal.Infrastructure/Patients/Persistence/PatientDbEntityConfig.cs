using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientPortal.Domain.Patients;
using PatientPortal.Infrastructure.Common.Persistence.ComplexProperties;
using PatientPortal.Infrastructure.Common.Persistence.Extensions;
using PatientPortal.Infrastructure.Common.Persistence.ValueConverters;
using PatientPortal.Persistence.Abstractions.Patients;

namespace PatientPortal.Infrastructure.Patients.Persistence;

public class PatientDbEntityConfig : IEntityTypeConfiguration<PatientDbEntity>
{
    public void Configure(EntityTypeBuilder<PatientDbEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
           .HasTypedIdConversion(PatientId.FromGuid, id => id);

        builder.ComplexProperty(x => x.Name,
            x => x.ConfigurePersonName());

        builder.Property(p => p.Gender)
            .IsRequired();

        builder.Property(p => p.Age)
            .IsRequired();
    }
}
