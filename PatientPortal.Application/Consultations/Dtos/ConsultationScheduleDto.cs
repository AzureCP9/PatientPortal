using NodaTime;
using PatientPortal.Application.Consultations.Dtos;
using PatientPortal.Domain.Consultations;

namespace PatientPortal.Application.Patients.Dtos;

// could be polymorphic in the future, but leave as nullable fields for
// simplicity for now
public record ConsultationScheduleDto(
    DateTimeOffset Start,
    DateTimeOffset End,
    int DurationInMinutes,
    DateTimeOffset? CancelledAt = null,
    string? CancellationReason = null);

public static class ConsultationScheduleDtoExtensions
{
    public static ConsultationScheduleDto ToDto(
        this ConsultationSchedule self) =>
            self switch
            {
                ConsultationSchedule.Scheduled s => new ConsultationScheduleDto(
                    s.Start.ToDateTimeOffset(),
                    s.End.ToDateTimeOffset(),
                    (int)s.Duration.TotalMinutes),

                ConsultationSchedule.Cancelled c => new ConsultationScheduleDto(
                    c.Start.ToDateTimeOffset(),
                    c.End.ToDateTimeOffset(),
                    (int)c.Duration.TotalMinutes,
                    c.CancelledAt.ToDateTimeOffset(),
                    CancellationReason: c.Reason
                ),

                _ => throw new ArgumentOutOfRangeException(
                    nameof(self),
                    self.GetType(),
                    "Unknown schedule type")
            };
}