using FluentResults;
using System.Reflection.Metadata.Ecma335;

namespace PatientPortal.Domain.Values;

public readonly record struct Age
{
    public int Value { get; }

    private Age(int value) => Value = value;

    public static Result<Age> TryCreate(int value) =>
        value is < 0 or > 130
            ? Result.Fail("Age must be between 0 and 130.")
            : new Age(value);

    public override string ToString() => Value.ToString();

    public static implicit operator int(Age age) => age.Value;
    public static explicit operator Age(int value) => Age.TryCreate(value).Value;
}