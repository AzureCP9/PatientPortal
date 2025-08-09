using PatientPortal.Application.Common.Dtos;
using PatientPortal.Domain.Values;

namespace PatientPortal.Application.Patients.Dtos;

public record CreatePatientRequestDto(
    PersonNameDto Name,
    GenderEnum Gender,
    int Age);