using FluentResults;
using Microsoft.AspNetCore.Mvc;
using PatientPortal.Common.Results.Errors;

namespace PatientPortal.Api.Common.Endpoints;

public static class MinimalApiResultTransformer
{
    public static IResult ToEndpointResult<T>(this Result<T> self)
    {
        if (self.IsSuccess)
            return Results.Ok(self.Value);

        return TransformFailedResult(self.Errors);
    }

    public static IResult ToEndpointResult(this Result self)
    {
        if (self.IsSuccess)
            return Results.NoContent();

        return TransformFailedResult(self.Errors);
    }

    public static IResult ToCreatedEndpointResult<T>(
        this Result<T> self,
        Func<T, string> locationFactory)
    {
        if (self.IsSuccess)
            return Results.Created(locationFactory(self.Value), self.Value);

        return self.ToEndpointResult();
    }

    private static IResult TransformFailedResult(
        IReadOnlyCollection<IError> errors)
    {
        if (errors.Count == 1)
            return Results.Problem(CreateProblemDetails(errors.First()));

        return Results.Problem(CreateMultipleErrorProblemDetails(errors));
    }

    private static ProblemDetails CreateProblemDetails(IError error)
    {
        var statusCode = GetStatusCodeForError(error);

        return new ProblemDetails
        {
            Title = error.GetType().Name,
            Status = statusCode,
            Detail = error.Message,
            Type = $"https://httpstatuses.com/{statusCode}"
        };
    }

    private static ProblemDetails CreateMultipleErrorProblemDetails(
        IReadOnlyCollection<IError> errors)
    {
        return new ProblemDetails
        {
            Title = "Multiple errors occurred",
            Status = StatusCodes.Status400BadRequest,
            Detail = 
                "The request could not be processed due to multiple errors.",
            Type = "https://httpstatuses.com/400",
            Extensions =
            {
                ["errors"] = errors.Select(e => e.Message).ToArray()
            }
        };
    }

    private static int GetStatusCodeForError(IError error) =>
        error switch
        {
            NotFoundError => StatusCodes.Status404NotFound,
            ConflictError => StatusCodes.Status409Conflict,
            // add more as needed, 401, etc
            _ => StatusCodes.Status400BadRequest
        };
}
