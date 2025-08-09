using NodaTime;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Consultations;
using PatientPortal.Domain.Patients;
using PatientPortal.Domain.Values;
using PatientPortal.Infrastructure.Common.Persistence;
using PatientPortal.Persistence.Abstractions.Patients;

namespace PatientPortal.Persistence.Abstractions.Consultations;

public class ConsultationDbEntity
{
    public required ConsultationId Id { get; set; }
    public required PatientId PatientId { get; set; }
    public PatientDbEntity Patient { get; set; } = 
        NavigationProperty.Single<PatientDbEntity>();

    public required Instant ScheduleStart { get; set; }
    public required Duration ScheduleDuration { get; set; }
    public required Instant ScheduleEnd { get; set; }
    public Instant? ScheduleCancelledAt { get; set; }
    public string? ScheduleCancellationReason { get; set; }
    public List<ConsultationAttachmentDbRepresentation> Attachments { get; set; } = [];
    public string? Notes { get; set; }
}

public static class ConsultationDbEntityExtensions
{
    public static ConsultationDbEntity ToDbEntity(this Consultation self)
    {
        var tuple = self.Schedule switch
        {
            ConsultationSchedule.Cancelled cancelled =>
                (cancelled.CancelledAt, (string)cancelled.Reason),
            _ => ((Instant?)null, (string?)null)
        };

        (Instant? cancelledAt, string? cancellationReason) = tuple;

        return new ConsultationDbEntity
        {
            Id = self.Id,
            PatientId = self.PatientId,
            ScheduleStart = self.Schedule.Start,
            ScheduleDuration = self.Schedule.Duration,
            ScheduleEnd = self.Schedule.Start + self.Schedule.Duration,
            ScheduleCancelledAt = cancelledAt,
            ScheduleCancellationReason = cancellationReason,
            Attachments = self.Attachments
                .Select(a => new ConsultationAttachmentDbRepresentation 
                {
                    Uri = a 
                })
                .ToList(),
            Notes = self.Notes
        };
    }

    public static Consultation ToDomain(this ConsultationDbEntity self)
    {
        var timeBlock = new TimeBlock(
            self.ScheduleStart, self.ScheduleDuration);

        ConsultationSchedule schedule = 
            (self.ScheduleCancelledAt, self.ScheduleCancellationReason) switch
            {
                (Instant cancelledAt, string reason) 
                    when !string.IsNullOrEmpty(reason) =>
                        new ConsultationSchedule.Cancelled(
                            timeBlock,
                            cancelledAt,
                            (NonEmptyString)reason),

                _ => new ConsultationSchedule.Scheduled(timeBlock)
            };

        return new Consultation(
            id: self.Id,
            patientId: self.PatientId,
            schedule: schedule,
            attachments: ConsultationAttachments.TryCreate(
                self.Attachments.Select(a => a.Uri)).Value,
            notes: self.Notes
        );
    }

    public static void UpdateFrom(
        this ConsultationDbEntity self,
        Consultation updated)
    {
        self.ScheduleStart = updated.Schedule.Start;
        self.ScheduleDuration = updated.Schedule.Duration;
        self.ScheduleEnd = updated.Schedule.Start + updated.Schedule.Duration;
        self.Notes = updated.Notes;
        self.Attachments = updated.Attachments
            .Select(a => new ConsultationAttachmentDbRepresentation { Uri = a })
            .ToList();

        self.ScheduleCancelledAt = null;
        self.ScheduleCancellationReason = null;

        if (updated.Schedule is ConsultationSchedule.Cancelled cancelled)
        {
            self.ScheduleCancelledAt = cancelled.CancelledAt;
            self.ScheduleCancellationReason = (string)cancelled.Reason;
        }
    }
}
