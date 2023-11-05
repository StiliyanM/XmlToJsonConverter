using System.Text.Json;

namespace XmlToJsonConverter.Web.Models;

public record ErrorDetails(int StatusCode, string Message)
{
    public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
}
