using PatientPortal.Domain.Patients;
using PatientPortal.Domain.Values;

namespace PatientPortal.Persistence.Abstractions.Patients;

public class PatientDbEntity
{
    public required PatientId Id { get; set; }
    public required PersonName Name { get; set; }
    public required Gender Gender { get; set; }
    public required Age Age { get; set; }
}

public static class PatientDbEntityExtensions
{
    public static PatientDbEntity ToDbEntity(this Patient self) => 
        new PatientDbEntity()
        {
            Id = self.Id,
            Name = self.Name,
            Gender = self.Gender,
            Age = self.Age
        };

    public static Patient ToDomain(this PatientDbEntity self) =>
        Patient.Create(
            new(
                Id: self.Id,
                Name: self.Name,
                Gender: self.Gender,
                Age: self.Age));

    public static void UpdateFrom(this PatientDbEntity entity, Patient domain)
    {
        entity.Name = domain.Name;
        entity.Gender = domain.Gender;
        entity.Age = domain.Age;
    }
}
