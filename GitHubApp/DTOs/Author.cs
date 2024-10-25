using System.Text.Json.Serialization;

namespace GitHubApp.DTOs;

public record Author(
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("name")] string Name
);
