using FluentResults;

namespace PatientPortal.Domain.Common;

public record struct AbsoluteUri
{
    public Uri Value { get; }
    private AbsoluteUri(Uri value) => Value = value;

    public static Result<AbsoluteUri> TryCreate(Uri value)
    {
        if (!value.IsAbsoluteUri)
            return Result.Fail("Uri must be absolute.");

        return new AbsoluteUri(value);
    }

    public static Result<AbsoluteUri> TryCreate(string value) =>
        Uri.TryCreate(value, UriKind.Absolute, out var uri)
           ? TryCreate(uri)
           : Result.Fail("Uri must be absolute.");

    public static implicit operator Uri(AbsoluteUri value) => value.Value;
    public static implicit operator string(AbsoluteUri value) => 
        value.Value.ToString();
    override public string ToString() => Value.ToString();
}