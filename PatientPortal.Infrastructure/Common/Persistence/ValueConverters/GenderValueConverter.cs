
using PatientPortal.Domain.Values;

namespace PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

public class GenderValueConverter() : EnumValueConverter<Gender, GenderEnum>(
     x => x.ToEnum(),
     x => x.ToGender()
 );