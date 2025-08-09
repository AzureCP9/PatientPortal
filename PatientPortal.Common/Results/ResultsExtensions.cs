using FluentResults;

namespace PatientPortal.Common.Results;

public delegate string ResultErrorFormatter(IEnumerable<IError> errors);

public static class ResultErrorformatters
{
    public static ResultErrorFormatter DefaultFormatter => 
        CommaSeparatedMessages;

    public static ResultErrorFormatter CommaSeparatedMessages => 
        (IEnumerable<IError> errors) =>
            errors.Count() == 1
                ? errors.First().Message
                : string.Join(", ", errors.Select(x => x.Message));
}

public static class ResultsExtensions
{
    public static T ValueOrThrow<T>(
        this Result<T> self,
        ResultErrorFormatter? formatter = null)
    {
        if (self.IsFailed)
        {
            var effectiveFormatter =
                formatter ?? ResultErrorformatters.DefaultFormatter;

            throw new InvalidOperationException(
                effectiveFormatter(self.Errors));
        }

        return self.Value;
    }
}