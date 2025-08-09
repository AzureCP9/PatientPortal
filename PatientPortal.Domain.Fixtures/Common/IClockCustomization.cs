using NodaTime;
using NodaTime.Testing;

namespace PatientPortal.Domain.Fixtures.Common;

public class IClockCustomization(Instant? withCurrentInstant = null)
    : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<IClock>(composer =>
            composer.FromFactory(() => new FakeClock(
                initial: withCurrentInstant
                ?? Instant.FromUtc(2021, 1, 1, 0, 0))));
    }
}