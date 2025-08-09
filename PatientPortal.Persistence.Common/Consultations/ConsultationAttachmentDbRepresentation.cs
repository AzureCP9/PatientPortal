using PatientPortal.Domain.Common;

namespace PatientPortal.Persistence.Abstractions.Consultations;

public class ConsultationAttachmentDbRepresentation
{
    public required AbsoluteUri Uri { get; set; }
}