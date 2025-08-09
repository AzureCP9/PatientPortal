using Bogus;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Values;

namespace PatientPortal.Domain.Fixtures.Values;

public class PersonNameCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var faker = new Faker();

        fixture.Register(() =>
        {
            var first = (NonEmptyString)faker.Name.FirstName();
            var last = (NonEmptyString)faker.Name.LastName();

            return new PersonName(first, last);
        });
    }
}