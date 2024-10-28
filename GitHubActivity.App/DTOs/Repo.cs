using System.Text.Json.Serialization;

namespace GithubActivity.App.DTOs;

public record Repo(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url")] string Url
);
