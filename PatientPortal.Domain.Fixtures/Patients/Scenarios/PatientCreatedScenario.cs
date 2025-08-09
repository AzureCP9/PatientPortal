using PatientPortal.Domain.Patients;

namespace PatientPortal.Domain.Fixtures.Patients.Scenarios;

public record PatientCreatedScenario(
    CreatePatient Input,
    Patient Patient);