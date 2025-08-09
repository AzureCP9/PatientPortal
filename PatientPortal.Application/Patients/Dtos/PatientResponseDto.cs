using PatientPortal.Application.Common.Dtos;
using PatientPortal.Domain.Patients;
using PatientPortal.Domain.Values;

namespace PatientPortal.Application.Patients.Dtos;

public record PatientResponseDto(
    Guid Id,
    PersonNameDto Name,
    GenderEnum Gender,
    int Age);

public static class PatientDtoExtensions
{
    public static PatientResponseDto ToDto(this Patient self) => 
        new(
            Id: self.Id.Value,
            Name: new PersonNameDto(
                FirstName: self.Name.FirstName,
                MiddleNames: self.Name.MiddleNames
                    .Select(n => n.Value)
                    .ToList(),
                LastName: self.Name.LastName
            ),
            Gender: self.Gender.ToEnum(),
            Age: self.Age);
}