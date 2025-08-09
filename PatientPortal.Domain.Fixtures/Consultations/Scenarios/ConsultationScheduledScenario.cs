using PatientPortal.Domain.Consultations;
using PatientPortal.Domain.Patients;

namespace PatientPortal.Domain.Fixtures.Consultations.Scenarios;

public record ConsultationScheduledScenario(
    ScheduleConsultation Input,
    Consultation Consultation,
    Patient Patient);