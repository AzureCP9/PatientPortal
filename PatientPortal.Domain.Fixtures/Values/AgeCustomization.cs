using Bogus;
using PatientPortal.Domain.Values;

namespace PatientPortal.Domain.Fixtures.Values;

public class AgeCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var faker = new Faker();

        fixture.Customize<Age>(composer =>
            composer.FromFactory(() =>
                (Age)faker.Random.Int(0, 130)
            )
        );
    }
}
