
using PatientPortal.Domain.Common;

namespace PatientPortal.Domain.Fixtures.Values;

public class AbsoluteUriCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<AbsoluteUri>(composer => composer.FromFactory(() => 
            AbsoluteUri.TryCreate("https://placecats.com/300/200").Value));
    }
}