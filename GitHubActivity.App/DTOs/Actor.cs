using System.Text.Json.Serialization;

namespace GithubActivity.App.DTOs;

public record Actor(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("display_login")] string DisplayLogin,
    [property: JsonPropertyName("gravatar_id")] string GravatarId,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("avatar_url")] string AvatarUrl
);
