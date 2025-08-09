using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

namespace PatientPortal.Infrastructure.Common.Persistence.Extensions;

public static class PropertyBuilderTypedIdExtensions
{
    public static PropertyBuilder<TTypedId> HasTypedIdConversion<TTypedId>(
        this PropertyBuilder<TTypedId> self,
        Func<Guid, TTypedId> factory,
        Func<TTypedId, Guid> extractor)
        where TTypedId : struct =>
            self.HasConversion(TypedIdValueConverter.For(factory, extractor))
                .ValueGeneratedNever();
}