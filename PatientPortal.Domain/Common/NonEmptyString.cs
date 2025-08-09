using FluentResults;
using FluentValidation;
using PatientPortal.Common.Results;

namespace PatientPortal.Domain.Common;

public readonly record struct NonEmptyString
{
    public string Value { get; }

    private NonEmptyString(string value) => Value = value;

    public static Result<NonEmptyString> TryCreate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result.Fail("String must not be null or empty.");

        return new NonEmptyString(input.Trim());
    }

    public override string ToString() => Value;

    public static implicit operator string(NonEmptyString s) => s.Value;
    public static explicit operator NonEmptyString(string s) => 
        TryCreate(s).ValueOrThrow();
}