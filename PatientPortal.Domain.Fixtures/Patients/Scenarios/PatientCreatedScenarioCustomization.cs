using PatientPortal.Domain.Patients;
using PatientPortal.Domain.Values;

namespace PatientPortal.Domain.Fixtures.Patients.Scenarios;

public class PatientCreatedScenarioCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<PatientCreatedScenario>(composer =>
            composer.FromFactory(() =>
            {
                var createPatient = fixture.Create<CreatePatient>() with
                {
                    Id = PatientId.Create(),
                    Name = fixture.Create<PersonName>(),
                    Gender = fixture.Create<Gender>(),
                    Age = fixture.Create<Age>()
                };

                var patient = Patient.Create(createPatient);

                return new PatientCreatedScenario(
                    Input: createPatient,
                    Patient: patient
                );
            })
            .OmitAutoProperties()
        );
    }
}