using System.Text.Json.Serialization;

namespace GithubActivity.App.DTOs;

public record Issue([property: JsonPropertyName("title")] string Title);