using PatientPortal.Domain.Fixtures.Common;
using PatientPortal.Domain.Fixtures.Consultations;
using PatientPortal.Domain.Fixtures.Consultations.Scenarios;
using PatientPortal.Domain.Fixtures.Patients.Scenarios;

namespace PatientPortal.Domain.Fixtures.Patients;

public static class FixtureExtensions
{
    public static IFixture WithPatientCustomizations(this IFixture self)
    {
        self
            .Customize(new PatientCreatedScenarioCustomization());

        return self;
    }
}