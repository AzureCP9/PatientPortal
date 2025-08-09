using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PatientPortal.Domain.Common;

namespace PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

public class AbsoluteUriValueConverter() : ValueConverter<AbsoluteUri, string>(
    x => x.Value.ToString(),
    x => AbsoluteUri.TryCreate(x).Value);