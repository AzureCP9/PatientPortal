namespace PatientPortal.Application.Consultations.Dtos;

public record ScheduleConsultationRequestDto(
    Guid PatientId,
    DateTimeOffset Start,
    int DurationInMinutes,
    List<string> AttachmentUris,
    string? Notes = null);