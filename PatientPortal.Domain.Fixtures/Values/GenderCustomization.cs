using PatientPortal.Domain.Values;

namespace PatientPortal.Domain.Fixtures.Values;

public class GenderCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Gender>(c =>
            c.FromFactory(() => 
                fixture.Create<bool>() 
                    ? new Gender.Male() 
                    : new Gender.Female())
             .OmitAutoProperties());
    }
}