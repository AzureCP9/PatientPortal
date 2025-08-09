using PatientPortal.Domain.Fixtures.Common;
using PatientPortal.Domain.Fixtures.Consultations.Scenarios;

namespace PatientPortal.Domain.Fixtures.Consultations;

public static class FixtureExtensions
{
    public static IFixture WithConsultationCustomizations(this IFixture self)
    {
        self
            .Customize(new ConsultationScheduleCustomization())
            .Customize(new ConsultationAttachmentsCustomization())
            .Customize(new ConsultationScheduledScenarioCustomization());

        return self;
    }
}