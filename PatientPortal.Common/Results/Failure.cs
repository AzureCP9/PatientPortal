using FluentResults;
using PatientPortal.Common.Results.Errors;

namespace PatientPortal.Common.Results;

public static class Failure
{
    public static Result NotFound(string message, object? identifier) =>
        Result.Fail(new NotFoundError(message, identifier));

    public static Result Conflict(string message) =>
        Result.Fail(new ConflictError(message));
}