using System.Text.Json.Serialization;

namespace GitHubApp.DTOs;

public record Org(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("gravatar_id")] string GravatarId,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("avatar_url")] string AvatarUrl
);