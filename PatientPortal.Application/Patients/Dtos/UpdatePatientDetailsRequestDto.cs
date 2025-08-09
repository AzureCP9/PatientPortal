using PatientPortal.Application.Common.Dtos;
using PatientPortal.Domain.Values;

namespace PatientPortal.Application.Patients.Dtos;

public record UpdatePatientDetailsRequestDto(
    PersonNameDto Name,
    GenderEnum Gender,
    int Age);