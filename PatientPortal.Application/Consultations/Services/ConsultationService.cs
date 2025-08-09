using FluentResults;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using PatientPortal.Application.Consultations.Dtos;
using PatientPortal.Application.Consultations.Services.Interfaces;
using PatientPortal.Common.Results;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Consultations;
using PatientPortal.Domain.Patients;
using PatientPortal.Domain.Values;
using PatientPortal.Persistence.Abstractions.Common.Interfaces;
using PatientPortal.Persistence.Abstractions.Consultations;

namespace PatientPortal.Application.Consultations.Services;

public class ConsultationService : IConsultationService
{
    private readonly IPatientPortalDbContext _dbContext;
    private readonly IClock _clock;

    public ConsultationService(IPatientPortalDbContext dbContext, IClock clock)
    {
        _dbContext = dbContext;
        _clock = clock;
    }

    public async Task<Result<ConsultationResponseDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var consultationId = ConsultationId.FromGuid(id);

        var row = await _dbContext.Consultations
            .AsNoTracking()
            .Where(c => c.Id == consultationId)
            .Select(c => new
            {
                Entity = c,
                PatientFirstLast = c.Patient.Name.FirstAndLast()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (row is null)
            return Failure.NotFound("Consultation not found.", id);

        return Result.Ok(row.Entity.ToDomain().ToDto(row.PatientFirstLast));
    }

    // TODO: pagination, operational window?
    public async Task<Result<List<ConsultationResponseDto>>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var rows = await _dbContext.Consultations
            .AsNoTracking()
            .OrderBy(c => c.ScheduleStart)
            .Select(c => new
            {
                Entity = c,
                PatientFirstLast = c.Patient.Name.FirstAndLast()
            })
            .ToListAsync(cancellationToken);

        var dtos = rows
            .Select(x => x.Entity.ToDomain().ToDto(x.PatientFirstLast))
            .ToList();

        return Result.Ok(dtos);
    }

    // we assume for now that multiple consultations are possible for the same
    // time block as you'd have multiple doctors, but we prevent multiple
    // bookings for the same patient
    public async Task<Result<ScheduleConsultationResponseDto>> ScheduleAsync(
        ScheduleConsultationRequestDto request,
        CancellationToken cancellationToken)
    {
        var id = ConsultationId.Create();
        var now = _clock.GetCurrentInstant();
        var patientId = PatientId.FromGuid(request.PatientId);

        var patientExists = await _dbContext.Patients
            .AnyAsync(p => p.Id == patientId, cancellationToken);

        if (!patientExists)
            return Failure.NotFound("Patient not found.", request.PatientId);

        var schedule = new ConsultationSchedule.Scheduled(new(
            Start: Instant.FromDateTimeOffset(request.Start),
            Duration: Duration.FromMinutes(request.DurationInMinutes)));

        var hasOverlap = await PatientHasScheduleConflictAsync(
            patientId, schedule, cancellationToken);

        if (hasOverlap)
            return Failure.Conflict("Patient already has a " +
                "Consultation scheduled during this time.");


        var attachmentsResult = ConsultationAttachments.TryCreate(
            request.AttachmentUris);

        if (attachmentsResult.IsFailed)
            return attachmentsResult.ToResult();

        var consultationResult = Consultation.TrySchedule(new(
            Now: now,
            Id: id,
            PatientId: patientId,
            Schedule: schedule,
            Attachments: attachmentsResult.Value,
            Notes: request.Notes));

        if (consultationResult.IsFailed)
            return consultationResult.ToResult();

        _dbContext.Consultations.Add(consultationResult.Value.ToDbEntity());

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok(new ScheduleConsultationResponseDto(id));
    }

    public async Task<Result> UpdateDetailsAsync(
        Guid id,
        UpdateConsultationDetailsRequestDto request,
        CancellationToken cancellationToken)
    {
        var consultationDbEntity = await _dbContext.Consultations.FindAsync(
            keyValues: [ConsultationId.FromGuid(id)],
            cancellationToken: cancellationToken);

        if (consultationDbEntity is null)
            return Failure.NotFound("Consultation not found.", id);

        var now = _clock.GetCurrentInstant();

        var schedule = new ConsultationSchedule.Scheduled(new(
            Start: Instant.FromDateTimeOffset(request.Start),
            Duration: Duration.FromMinutes(request.DurationInMinutes)));

        var hasOverlap = await PatientHasScheduleConflictAsync(
            consultationDbEntity.PatientId,
            schedule,
            cancellationToken,
            excludingConsultationId: consultationDbEntity.Id);

        if (hasOverlap)
            return Failure.Conflict("Patient already has a " +
                "Consultation scheduled during this time.");

        var attachmentsResult = ConsultationAttachments.TryCreate(
            request.AttachmentUris);

        if (attachmentsResult.IsFailed) return attachmentsResult.ToResult();

        var updatedResult = consultationDbEntity.ToDomain()
            .TryUpdateDetails(new(
                Now: now,
                Schedule: schedule,
                Attachments: attachmentsResult.Value,
                Notes: request.Notes));

        if (updatedResult.IsFailed) return updatedResult.ToResult();

        consultationDbEntity.UpdateFrom(updatedResult.Value);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private async Task<bool> PatientHasScheduleConflictAsync(
        PatientId patientId,
        ConsultationSchedule.Scheduled schedule,
        CancellationToken cancellationToken,
        ConsultationId? excludingConsultationId = null)
    {
        var start = schedule.Start;
        var end = schedule.Start + schedule.Duration;

        var query = _dbContext.Consultations
            .Where(c => c.PatientId == patientId)
            .Where(c => c.ScheduleCancelledAt == null);

        if (excludingConsultationId is not null)
            query = query.Where(c => c.Id != excludingConsultationId);

        return await query
            .Where(c =>
                c.ScheduleStart < end &&
                start < c.ScheduleEnd)
            .AnyAsync(cancellationToken);
    }

    public async Task<Result> CancelAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var consultationDb = await _dbContext.Consultations.FindAsync(
            keyValues: [ConsultationId.FromGuid(id)],
            cancellationToken: cancellationToken);

        if (consultationDb is null)
            return Failure.NotFound("Consultation not found.", id);

        var now = _clock.GetCurrentInstant();

        var cancelResult = consultationDb.ToDomain()
            .TryCancelConsultation(new CancelConsultation(
                CancelledAt: now,
                // future implementation would have this as a custom message from the request
                Reason: (NonEmptyString)"Cancelled by user."));

        if (cancelResult.IsFailed) return cancelResult.ToResult();

        consultationDb.UpdateFrom(cancelResult.Value);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
