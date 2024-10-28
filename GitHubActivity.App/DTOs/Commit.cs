using System.Text.Json.Serialization;

namespace GithubActivity.App.DTOs;

public record Commit(
    [property: JsonPropertyName("sha")] string Sha,
    [property: JsonPropertyName("author")] Author Author,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("distinct")] bool Distinct,
    [property: JsonPropertyName("url")] string Url
);
