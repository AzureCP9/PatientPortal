using NodaTime;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Values;

namespace PatientPortal.Domain.Consultations;

public abstract record ConsultationSchedule(
    TimeBlock TimeBlock)
{
    public Instant Start => TimeBlock.Start;
    public Duration Duration => TimeBlock.Duration;
    public Instant End => TimeBlock.End;

    public sealed record Scheduled(TimeBlock TimeBlock) 
        : ConsultationSchedule(TimeBlock);

    public sealed record Cancelled(
        TimeBlock TimeBlock, 
        Instant CancelledAt,
        NonEmptyString Reason)
        : ConsultationSchedule(TimeBlock);
}