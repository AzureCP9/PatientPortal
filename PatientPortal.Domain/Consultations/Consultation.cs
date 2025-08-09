using FluentResults;
using NodaTime;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Patients;

namespace PatientPortal.Domain.Consultations;

public record ScheduleConsultation(
    Instant Now,
    ConsultationId Id,
    PatientId PatientId,
    ConsultationSchedule.Scheduled Schedule,
    ConsultationAttachments Attachments,
    string? Notes);

public record UpdateConsultationDetails(
    Instant Now,
    ConsultationSchedule.Scheduled Schedule,
    ConsultationAttachments Attachments,
    string? Notes = null);

public record CancelConsultation(
    Instant CancelledAt,
    NonEmptyString Reason);

public record Consultation
{
    public ConsultationId Id { get; init; }
    public PatientId PatientId { get; init; }
    public ConsultationSchedule Schedule { get; init; }
    public ConsultationAttachments Attachments { get; init; }
    public string? Notes { get; init; }

    // reasonable consultation duration placeholders
    private static readonly Duration MinDuration = Duration.FromMinutes(15);
    private static readonly Duration MaxDuration = Duration.FromHours(2);

    public bool IsCancelled => Schedule is ConsultationSchedule.Cancelled;

    public Consultation(
        ConsultationId id,
        PatientId patientId,
        ConsultationSchedule schedule,
        ConsultationAttachments attachments,
        string? notes = null)
    {
        Id = id;
        PatientId = patientId;
        Schedule = schedule;
        Attachments = attachments;
        Notes = notes;
    }

    public static Result<Consultation> TrySchedule(
        ScheduleConsultation input)
    {
        var scheduleResult = ValidateSchedule(input.Now, input.Schedule);

        if (scheduleResult.IsFailed) return scheduleResult.ToResult();

        return new Consultation(
            input.Id,
            input.PatientId,
            input.Schedule,
            input.Attachments,
            input.Notes);
    }

    // PatientId is intentionally immutable to reflect real-world constraints.
    // If reassignment is ever required, cancellation + new consultation is recommended.
    // future feature could be allow reassign prior to consultation start with a TryReassignPatient (outside of scope)

    public Result<Consultation> TryUpdateDetails(
        UpdateConsultationDetails input)
    {
        if (IsCancelled)
            return Result.Fail("Cannot update a cancelled Consultation.");

        var currentSchedule = (ConsultationSchedule.Scheduled)Schedule;

        var scheduleResult = ValidateSchedule(
            input.Now,
            input.Schedule,
            currentSchedule);

        if (scheduleResult.IsFailed) return scheduleResult.ToResult();

        return this with
        {
            Schedule = scheduleResult.Value,
            Attachments = input.Attachments,
            Notes = input.Notes
        };
    }

    private static Result<ConsultationSchedule> ValidateSchedule(
        Instant now,
        ConsultationSchedule.Scheduled newSchedule,
        ConsultationSchedule.Scheduled? currentSchedule = null)
    {
        // this is a hack for the takehome so I don't have to split scheduling and editing notes/attachments
        // in separate commands/service calls which would be richer, but unnecessary for this project
        if ((currentSchedule is null || !newSchedule.Equals(currentSchedule))
            && newSchedule.Start < now)
        {
            return Result.Fail("Cannot schedule Consultation in the past.");
        }

        if (newSchedule.Duration < MinDuration)
            return Result.Fail($"Consultation must be a minimum of " +
                $"'{MinDuration.TotalMinutes}' minutes.");

        if (newSchedule.Duration > MaxDuration)
            return Result.Fail($"Consultation must be a maximum of " +
                $"'{MaxDuration.TotalHours}' hours.");

        return newSchedule;
    }

    public Result<Consultation> TryCancelConsultation(
        CancelConsultation input)
    {
        if (IsCancelled)
            return Result.Fail("Consultation is already cancelled.");

        if (input.CancelledAt >= Schedule.End)
            return Result.Fail("Cannot cancel Consultation that has ended.");

        return this with
        {
            Schedule = new ConsultationSchedule.Cancelled(
                TimeBlock: Schedule.TimeBlock,
                CancelledAt: input.CancelledAt,
                Reason: input.Reason)
        };
    }

}
