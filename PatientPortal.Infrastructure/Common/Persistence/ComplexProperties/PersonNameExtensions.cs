using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientPortal.Domain.Values;
using PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

namespace PatientPortal.Infrastructure.Common.Persistence.ComplexProperties;

public static class PersonNameExtensions
{
    public static ComplexPropertyBuilder<PersonName> ConfigurePersonName(
        this ComplexPropertyBuilder<PersonName> self)
    {
        self.Property(n => n.FirstName)
            .HasMaxLength(DataStandards.NameMaxLength)
            .IsRequired();

        self.Property(n => n.LastName)
            .HasMaxLength(DataStandards.NameMaxLength)
            .IsRequired();

        self.Property(n => n.MiddleNames)
            .HasMaxLength(DataStandards.NameMaxLength)
            .IsRequired()
            .HasConversion(PersonNameValueConverters.MiddleNamesConverter)
            .Metadata.SetValueComparer(
                ValueComparers.ImmutableNonEmptyStringList);

        return self;
    }
}