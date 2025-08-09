namespace PatientPortal.Common.FileSystem;

public static class FileNameSanitizer
{
    public static string ReplaceInvalidFileNameChars(
        this string input, char replacement = '_')
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Join(string.Empty,
            input.Select(c => invalidChars.Contains(c) ? replacement : c));
    }
}
