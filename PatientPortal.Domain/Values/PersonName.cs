using PatientPortal.Domain.Common;
using System.Collections.Immutable;

namespace PatientPortal.Domain.Values;

public record PersonName
{
    public NonEmptyString FirstName { get; init; }
    public ImmutableList<NonEmptyString> MiddleNames { get; init; } = [];
    public NonEmptyString LastName { get; init; }

    public PersonName(NonEmptyString firstName, NonEmptyString lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public PersonName(
        NonEmptyString firstName,
        List<NonEmptyString> middleNames,
        NonEmptyString lastName)
    {
        FirstName = firstName;
        MiddleNames = middleNames.ToImmutableList();
        LastName = lastName;
    }
}

public static class PersonNameExtensions
{
    public static string FullName(this PersonName self) =>
        self.MiddleNames is null
            ? $"{self.FirstName.Value} {self.LastName.Value}"
            : $"{self.FirstName.Value} " +
            $"{string.Join(" ", self.MiddleNames.Select(mn => mn.Value))} " +
           $"{self.LastName.Value}";

    public static string FirstAndLast(this PersonName self) =>
        $"{self.FirstName} {self.LastName}";
}