using System.Text.Json.Serialization;

namespace GithubActivity.App.DTOs;

public record Author(
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("name")] string Name
);
