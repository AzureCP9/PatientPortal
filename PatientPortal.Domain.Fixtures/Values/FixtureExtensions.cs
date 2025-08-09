namespace PatientPortal.Domain.Fixtures.Values;

public static class FixtureExtensions
{
    public static IFixture WithValueCustomizations(this IFixture self)
    {
        self
            .Customize(new NonEmptyStringCustomization())
            .Customize(new AbsoluteUriCustomization())
            .Customize(new GenderCustomization())
            .Customize(new AgeCustomization())
            .Customize(new PersonNameCustomization());

        return self;
    }
}