using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PatientPortal.Domain.Common;

namespace PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

public class NonEmptyStringValueConverter() 
    : ValueConverter<NonEmptyString, string>(
        x => x.Value,
        x => (NonEmptyString)x);