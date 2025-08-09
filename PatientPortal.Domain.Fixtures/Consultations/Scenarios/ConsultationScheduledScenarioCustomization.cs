using Bogus;
using NodaTime;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Consultations;
using PatientPortal.Domain.Fixtures.Consultations.Scenarios;
using PatientPortal.Domain.Fixtures.Patients.Scenarios;
using PatientPortal.Domain.Patients;

namespace PatientPortal.Domain.Fixtures.Consultations.Scenarios;

public class ConsultationScheduledScenarioCustomization(
    ConsultationId? withId = null,
    PatientCreatedScenario? withPatientScenario = null,
    ConsultationSchedule.Scheduled? withSchedule = null,
    ConsultationAttachments? withAttachments = null,
    string? withNotes = null) 
    : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var faker = new Faker();

        fixture.Customize<ConsultationScheduledScenario>(composer =>
            composer.FromFactory(() =>
            {
                var patientScenario = withPatientScenario 
                    ?? fixture.Create<PatientCreatedScenario>();
                var patient = patientScenario.Patient;

                var consultationId = withId 
                    ?? fixture.Create<ConsultationId>();

                var clock = fixture.Create<IClock>();
                var now = clock.GetCurrentInstant();

                var schedule = withSchedule 
                    ?? fixture.Create<ConsultationSchedule.Scheduled>();

                var attachments = withAttachments 
                    ?? fixture.Create<ConsultationAttachments>();

                var randomizedNotes = fixture.Create<bool>()
                        ? faker.Lorem.Sentence()
                        : null;

                var notes = withNotes ?? randomizedNotes;

                var input = new ScheduleConsultation(
                    Now: now,
                    Id: consultationId,
                    PatientId: patient.Id,
                    Schedule: schedule,
                    Attachments: attachments,
                    Notes: notes
                );

                var consultation = Consultation.TrySchedule(input).Value;

                return new ConsultationScheduledScenario(
                    Input: input,
                    Consultation: consultation,
                    Patient: patient
                );
            }).OmitAutoProperties());
    }
}
