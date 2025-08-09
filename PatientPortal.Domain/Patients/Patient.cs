using PatientPortal.Domain.Values;

namespace PatientPortal.Domain.Patients;

public record CreatePatient(
    PatientId Id,
    PersonName Name,
    Gender Gender,
    Age Age);

public record UpdatePatientDetails(
    PersonName Name,
    Gender Gender,
    Age Age);

public record Patient
{
    public PatientId Id { get; init; }
    public PersonName Name { get; init; }
    public Gender Gender { get; init; }
    public Age Age { get; init; }

    public Patient(
        PatientId id,
        PersonName name,
        Gender gender,
        Age age)
    {
        Id = id;
        Name = name;
        Gender = gender;
        Age = age;
    }

    // we ignore patients with same age, name, and gender since we are only making a small concept demo
    public static Patient Create(CreatePatient input) =>
        new Patient(
            id: input.Id,
            name: input.Name,
            gender: input.Gender,
            age: input.Age);

    public Patient UpdatePersonalDetails(UpdatePatientDetails input) =>
        this with
        {
            Name = input.Name,
            Gender = input.Gender,
            Age = input.Age
        };

}