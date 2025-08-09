using PatientPortal.Domain.Fixtures.Values;

namespace PatientPortal.Domain.Fixtures.Common;

public static class FixtureExtensions
{
    public static IFixture WithCommonCustomizations(this IFixture self)
    {
        self.Customize(new IClockCustomization());

        return self;
    }
}