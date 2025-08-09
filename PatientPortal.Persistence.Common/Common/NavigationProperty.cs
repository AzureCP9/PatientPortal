namespace PatientPortal.Infrastructure.Common.Persistence;

public static class NavigationProperty
{
    public static T Single<T>() => default!;
    public static T? OptionalSingle<T>() => default;
    public static ICollection<T> Collection<T>() => new List<T>();
}