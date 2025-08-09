using PatientPortal.Domain.Common;

namespace PatientPortal.Domain.Patients;

public readonly record struct PatientId(TypedId Value)
{
    public static PatientId Create() => new(TypedId.Create());
    public static PatientId FromGuid(Guid guid) => new(TypedId.FromGuid(guid));

    public static implicit operator Guid(PatientId id) => id.Value;
    public static explicit operator PatientId(Guid value) => 
        new((TypedId)value);

    public override string ToString() => Value.ToString();
}