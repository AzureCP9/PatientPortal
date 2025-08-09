using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PatientPortal.Domain.Values;

namespace PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

public class AgeValueConverter : ValueConverter<Age, int>
{
    public AgeValueConverter()
        : base(
            age => age.Value,
            value => (Age)value)
    {
    }
}