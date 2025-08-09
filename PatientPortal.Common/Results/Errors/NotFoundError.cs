using FluentResults;

namespace PatientPortal.Common.Results.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string message, object? identifier) : base(message)
    {
        Identifier = identifier;
    }

    public object? Identifier { get; init; }
}