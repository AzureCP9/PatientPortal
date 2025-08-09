using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Extensions;

namespace PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

public class InstantValueConverter() 
    : ValueConverter<Instant?, DateTimeOffset?>(
        x => NullableInstantToDateTimeOffset(x),
        x => NullableDateTimeOffsetToInstant(x))
{
    private static DateTimeOffset? NullableInstantToDateTimeOffset(
        Instant? instant) => 
            instant?.ToDateTimeOffset();

    private static Instant? NullableDateTimeOffsetToInstant(
        DateTimeOffset? dateTimeOffset) => 
            dateTimeOffset?.ToInstant();
}