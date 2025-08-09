using FluentResults;

namespace PatientPortal.Common.Results.Errors;

public class ConflictError : Error
{
    public ConflictError(string message) : base(message) {}
}