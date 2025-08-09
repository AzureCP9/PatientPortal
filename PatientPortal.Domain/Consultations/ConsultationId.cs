using PatientPortal.Domain.Common;

namespace PatientPortal.Domain.Consultations;

public readonly record struct ConsultationId(TypedId Value)
{
    public static ConsultationId Create() => new(TypedId.Create());
    public static ConsultationId FromGuid(Guid guid) => 
        new(TypedId.FromGuid(guid));

    public static implicit operator Guid(ConsultationId id) => id.Value;
    public static explicit operator ConsultationId(Guid value) =>
        new((TypedId)value);

    public override string ToString() => Value.ToString();
}