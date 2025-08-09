using FluentResults;
using Microsoft.EntityFrameworkCore;
using PatientPortal.Application.Common.Dtos;
using PatientPortal.Application.Patients.Dtos;
using PatientPortal.Application.Patients.Services.Interfaces;
using PatientPortal.Common.Results;
using PatientPortal.Domain.Patients;
using PatientPortal.Domain.Values;
using PatientPortal.Persistence.Abstractions.Common.Interfaces;
using PatientPortal.Persistence.Abstractions.Patients;

namespace PatientPortal.Application.Patients.Services;

public class PatientService : IPatientService
{
    private readonly IPatientPortalDbContext _dbContext;

    public PatientService(IPatientPortalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PatientResponseDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var patientId = PatientId.FromGuid(id);

        var patientDbEntity = await _dbContext.Patients.FindAsync(
            keyValues: [patientId],
            cancellationToken);

        if (patientDbEntity is null)
            return Failure.NotFound("Patient not found.", id);

        return Result.Ok(patientDbEntity.ToDomain().ToDto());
    }

    // in the future, add an operational window or pagination
    public async Task<Result<List<PatientResponseDto>>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var patients = await _dbContext.Patients
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var patientDtos = patients
            .Select(p => p.ToDomain().ToDto())
            .ToList();

        return Result.Ok(patientDtos);
    }

    public async Task<Result<CreatePatientResponseDto>> CreateAsync(
        CreatePatientRequestDto request,
        CancellationToken cancellationToken)
    {
        var patientId = PatientId.Create();
        var personNameResult = request.Name.TryToPersonName();
        var ageResult = Age.TryCreate(request.Age);

        var results = Result.Merge(personNameResult, ageResult);

        if (results.IsFailed) 
            return results.ToResult<CreatePatientResponseDto>();

        var patient = Patient.Create(new CreatePatient(
            Id: patientId,
            Name: personNameResult.Value,
            Gender: request.Gender.ToGender(),
            Age: ageResult.Value));

        _dbContext.Patients.Add(patient.ToDbEntity());

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok(new CreatePatientResponseDto(patientId));
    }

    

    public async Task<Result> UpdateDetailsAsync(
        Guid id,
        UpdatePatientDetailsRequestDto request,
        CancellationToken cancellationToken)
    {
        var patientDbEntity = await _dbContext.Patients.FindAsync(
            keyValues: [PatientId.FromGuid(id)], 
            cancellationToken: cancellationToken);

        if (patientDbEntity is null)
            return Failure.NotFound("Patient not found.", id);

        var nameResult = request.Name.TryToPersonName();
        var ageResult = Age.TryCreate(request.Age);

        var results = Result.Merge(nameResult, ageResult);
        if (results.IsFailed) return results;

        var patient = patientDbEntity
            .ToDomain()
            .UpdatePersonalDetails(new(
                Name: nameResult.Value,
                Gender: request.Gender.ToGender(),
                Age: ageResult.Value));


        patientDbEntity.UpdateFrom(patient);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
