using System.Text.Json.Serialization;

namespace GithubActivity.App.DTOs;

public record PullRequest(
    [property: JsonPropertyName("url")] string Url
);