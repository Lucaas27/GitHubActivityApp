using System.Text.Json.Serialization;

namespace GithubActivity.App.DTOs;

public record Comment(
    [property: JsonPropertyName("body")] string Body
);