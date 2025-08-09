using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Extensions;

namespace PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

public class DurationValueConverter() : ValueConverter<Duration?, TimeSpan?>(
    x => NullableDurationToTimeSpan(x),
    x => NullableTimeSpanToDuration(x)
)
{
    private static TimeSpan? NullableDurationToTimeSpan(Duration? duration) => 
        duration?.ToTimeSpan();
    private static Duration? NullableTimeSpanToDuration(TimeSpan? duration) => 
        duration?.ToDuration();
}