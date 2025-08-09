using NodaTime;

namespace PatientPortal.Domain.Values;

public record TimeBlock(Instant Start, Duration Duration)
{
    public Instant End => Start.Plus(Duration);

    public bool IsOngoing(Instant now) =>
        now >= Start && now < End;

    public bool IsFuture(Instant now) =>
        now < Start;

    public bool IsPast(Instant now) =>
        now >= End;
}