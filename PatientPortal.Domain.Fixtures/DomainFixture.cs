using AutoFixture.AutoNSubstitute;
using PatientPortal.Domain.Fixtures.Common;
using PatientPortal.Domain.Fixtures.Consultations;
using PatientPortal.Domain.Fixtures.Patients;
using PatientPortal.Domain.Fixtures.Values;

namespace PatientPortal.Domain.Fixtures;

public static class DomainFixture
{
    public static IFixture Create() =>
        new Fixture()
            .Customize(new AutoNSubstituteCustomization())
            .WithCommonCustomizations()
            .WithValueCustomizations()
            .WithPatientCustomizations()
            .WithConsultationCustomizations();
}