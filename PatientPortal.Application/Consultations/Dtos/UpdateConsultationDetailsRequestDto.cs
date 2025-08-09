namespace PatientPortal.Application.Consultations.Dtos;

public record UpdateConsultationDetailsRequestDto(
    DateTimeOffset Start,
    int DurationInMinutes,
    List<string> AttachmentUris,
    string? Notes = null);