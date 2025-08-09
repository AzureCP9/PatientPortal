using FluentResults;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Values;

namespace PatientPortal.Application.Common.Dtos;

public record PersonNameDto(
    string FirstName,
    List<string> MiddleNames,
    string LastName);

public static class PersonNameDtoExtensions
{
    public static Result<PersonName> TryToPersonName(this PersonNameDto self)
    {
        var firstNameResult = NonEmptyString.TryCreate(self.FirstName);
        var lastNameResult = NonEmptyString.TryCreate(self.LastName);
        var middleNameResults = self.MiddleNames
            .Select(NonEmptyString.TryCreate)
            .ToList();

        var merged = Result.Merge(
            [firstNameResult, lastNameResult, .. middleNameResults]);

        if (merged.IsFailed) return merged.ToResult();

        return new PersonName(
            firstName: firstNameResult.Value,
            middleNames: middleNameResults.Select(r => r.Value).ToList(),
            lastName: lastNameResult.Value);
    }
}