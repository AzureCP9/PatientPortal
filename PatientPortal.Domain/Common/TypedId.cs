using RT.Comb;

namespace PatientPortal.Domain.Common;

public readonly record struct TypedId
{
    // prevent clustering issues for mssql which uses non-sequential GUIDs by default
    private static readonly ICombProvider _provider = Provider.Sql;
    public Guid Value { get; init; }
    private TypedId(Guid guid)
    {
        if (guid == Guid.Empty)
            throw new InvalidOperationException(
                "EntityId cannot have an empty Guid.");

        Value = guid;
    }

    public static TypedId Create() => new(_provider.Create());
    public static TypedId FromGuid(Guid guid) => new TypedId(guid);
    public override string ToString() => Value.ToString();

    public static implicit operator Guid(TypedId id) => id.Value;
    public static explicit operator TypedId(Guid value) => FromGuid(value);
}
