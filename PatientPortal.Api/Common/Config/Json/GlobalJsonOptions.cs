using System.Text.Json;
using System.Text.Json.Serialization;

namespace PatientPortal.Api.Common.Config.Json;

public static class GlobalJsonOptions
{
    public static readonly JsonSerializerOptions WebJsonOptions = new(
        JsonSerializerDefaults.Web)
    {
        Converters = {
            new JsonStringEnumConverter(),
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}