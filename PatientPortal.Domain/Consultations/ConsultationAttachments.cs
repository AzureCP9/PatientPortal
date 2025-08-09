using FluentResults;
using PatientPortal.Domain.Common;
using System.Collections;
using System.Collections.Immutable;

namespace PatientPortal.Domain.Consultations;

public sealed class ConsultationAttachments : IReadOnlyList<AbsoluteUri>
{
    private const int MaxUploadsAllowed = 5;
    private readonly ImmutableList<AbsoluteUri> _uploads;

    private ConsultationAttachments(ImmutableList<AbsoluteUri> uploads) =>
        _uploads = uploads;

    public static Result<ConsultationAttachments> TryCreate(
        IEnumerable<AbsoluteUri> uploads) =>
        uploads.Count() > MaxUploadsAllowed
            ? Result.Fail($"A Consultation can include up " +
                $"to {MaxUploadsAllowed} attachments.")
            : Result.Ok(new ConsultationAttachments(uploads.ToImmutableList()));

    public static Result<ConsultationAttachments> TryCreate(
        IEnumerable<string> uploadUris) =>
            uploadUris.Select(AbsoluteUri.TryCreate)
                .Merge()
                .Bind(TryCreate);

    public AbsoluteUri this[int index] => _uploads[index];

    public int Count => _uploads.Count;

    public IEnumerator<AbsoluteUri> GetEnumerator() => _uploads.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
