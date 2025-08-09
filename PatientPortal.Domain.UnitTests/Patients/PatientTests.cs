using AutoFixture;
using AwesomeAssertions;
using PatientPortal.Domain.Fixtures;
using PatientPortal.Domain.Patients;
using PatientPortal.Domain.Values;
using Xunit;

namespace PatientPortal.Domain.UnitTests.Patients;

public class PatientTests
{
    private readonly IFixture _fixture = DomainFixture.Create();

    [Fact]
    public void RegisterPatient_SetsIdentityAndDemographics()
    {
        var input = _fixture.Create<CreatePatient>();

        var patient = Patient.Create(input);

        patient.Id.Should().Be(input.Id);
        patient.Name.Should().Be(input.Name);
        patient.Gender.Should().Be(input.Gender);
        patient.Age.Should().Be(input.Age);
    }

    [Fact]
    public void UpdatePersonalDetails_ReplacesDetails_ButKeepsId()
    {
        var original = Patient.Create(_fixture.Create<CreatePatient>());
        var update = _fixture.Create<UpdatePatientDetails>();

        var updated = original.UpdatePersonalDetails(update);

        updated.Id.Should().Be(original.Id);
        updated.Name.Should().Be(update.Name);
        updated.Gender.Should().Be(update.Gender);
        updated.Age.Should().Be(update.Age);
    }

    [Fact]
    public void UpdatePersonalDetails_ReturnsNewEntityInstance()
    {
        var original = Patient.Create(_fixture.Create<CreatePatient>());
        var update = _fixture.Create<UpdatePatientDetails>();

        var updated = original.UpdatePersonalDetails(update);

        ReferenceEquals(updated, original).Should().BeFalse();
    }

    [Fact]
    public void UpdatePersonalDetails_WithNoChanges_IsIdempotent()
    {
        var original = Patient.Create(_fixture.Create<CreatePatient>());
        var update = new UpdatePatientDetails(original.Name, original.Gender, original.Age);

        var updated = original.UpdatePersonalDetails(update);

        updated.Should().Be(original);
        ReferenceEquals(updated, original).Should().BeFalse();
    }

    [Fact]
    public void UpdatePersonalDetails_DoesNotMutateOriginal()
    {
        var original = Patient.Create(_fixture.Create<CreatePatient>());
        var snapshot = original with { };

        var update = _fixture.Create<UpdatePatientDetails>();
        _ = original.UpdatePersonalDetails(update);

        original.Should().Be(snapshot);
    }
}
