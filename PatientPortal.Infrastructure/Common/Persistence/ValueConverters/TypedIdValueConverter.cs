using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

public static class TypedIdValueConverter
{
    public static ValueConverter<TTypedId, Guid> For<TTypedId>(
        Func<Guid, TTypedId> factory,
        Func<TTypedId, Guid> extractor)
        where TTypedId : struct => 
            new ValueConverter<TTypedId, Guid>(
                id => extractor(id),
                guid => factory(guid));
}