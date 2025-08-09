using FluentResults;
using PatientPortal.Application.Consultations.Dtos;

namespace PatientPortal.Application.Consultations.Services.Interfaces;

public interface IConsultationService
{
    Task<Result<ConsultationResponseDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    // TODO: create a summary dto for the list view, but out of scope currently
    Task<Result<List<ConsultationResponseDto>>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<Result<ScheduleConsultationResponseDto>> ScheduleAsync(
        ScheduleConsultationRequestDto request,
        CancellationToken cancellationToken);

    Task<Result> UpdateDetailsAsync(
        Guid id,
        UpdateConsultationDetailsRequestDto request,
        CancellationToken cancellationToken);

    Task<Result> CancelAsync(
        Guid id,
        CancellationToken cancellationToken);
}