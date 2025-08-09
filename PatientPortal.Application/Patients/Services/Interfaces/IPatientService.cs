using FluentResults;
using PatientPortal.Application.Patients.Dtos;

namespace PatientPortal.Application.Patients.Services.Interfaces;

public interface IPatientService
{
    Task<Result<PatientResponseDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<Result<List<PatientResponseDto>>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<Result<CreatePatientResponseDto>> CreateAsync(
        CreatePatientRequestDto request,
        CancellationToken cancellationToken);

    Task<Result> UpdateDetailsAsync(
        Guid id,
        UpdatePatientDetailsRequestDto request,
        CancellationToken cancellationToken);
}
