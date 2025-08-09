using PatientPortal.Application.Patients.Dtos;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Consultations;

namespace PatientPortal.Application.Consultations.Dtos;

public record ConsultationResponseDto(
    Guid Id,
    Guid PatientId,
    string PatientName,
    ConsultationScheduleDto Schedule,
    IReadOnlyList<string> AttachmentUris,
    string? Notes);

public static class ConsultationResponseDtoExtensions
{
    public static ConsultationResponseDto ToDto(
        this Consultation self,
        string patientName) =>
            new ConsultationResponseDto(
                Id: self.Id.Value,
                PatientId: self.PatientId.Value,
                PatientName: patientName,
                Schedule: self.Schedule.ToDto(),
                AttachmentUris:
                    self.Attachments.Select(a => a.Value.ToString()).ToList(),
                Notes:  self.Notes
            );
}