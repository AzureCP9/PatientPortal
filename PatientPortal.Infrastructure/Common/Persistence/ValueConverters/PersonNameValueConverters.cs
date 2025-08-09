using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PatientPortal.Domain.Common;
using System.Collections.Immutable;

namespace PatientPortal.Infrastructure.Common.Persistence.ValueConverters;

public static class PersonNameValueConverters
{
    public static readonly ValueConverter<ImmutableList<NonEmptyString>, string> MiddleNamesConverter =
        new(
            v => string.Join(',', v.Select(name => name.ToString())),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                  .Select(x => (NonEmptyString)x)
                  .ToImmutableList());
}
