using Bogus;
using PatientPortal.Domain.Common;

namespace PatientPortal.Domain.Fixtures.Values;

public class NonEmptyStringCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var faker = new Faker();

        fixture.Customize<NonEmptyString>(composer =>
            composer.FromFactory(() =>
                (NonEmptyString)faker.Lorem.Word()
            )
        );
    }
}