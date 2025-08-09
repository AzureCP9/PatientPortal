using Microsoft.EntityFrameworkCore.ChangeTracking;
using PatientPortal.Domain.Common;
using System.Collections.Immutable;

namespace PatientPortal.Infrastructure.Common.Persistence;

public static class ValueComparers
{
    public static readonly ValueComparer<ImmutableList<NonEmptyString>> ImmutableNonEmptyStringList =
        new(
            (c1, c2) =>
                c1 == null && c2 == null ||
                c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c == null
                ? 0
                : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToImmutableList());
}