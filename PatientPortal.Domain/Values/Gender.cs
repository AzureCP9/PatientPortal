namespace PatientPortal.Domain.Values;

public abstract record Gender
{
    public sealed record Male : Gender;
    public sealed record Female : Gender;
}

public enum GenderEnum
{
    Male,
    Female,
}


public static class GenderExtensions
{
    public static GenderEnum ToEnum(this Gender self) =>
        self switch
        {
            Gender.Male => GenderEnum.Male,
            Gender.Female => GenderEnum.Female,
            _ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
        };

    public static Gender ToGender(this GenderEnum self) =>
        self switch
        {
            GenderEnum.Male => new Gender.Male(),
            GenderEnum.Female => new Gender.Female(),
            _ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
        };
}