using System.Text.Json.Serialization;

namespace GithubActivity.App.DTOs;

public record Payload(
    [property: JsonPropertyName("repository_id")] int RepositoryId,
    [property: JsonPropertyName("push_id")] object PushId,
    [property: JsonPropertyName("size")] int Size,
    [property: JsonPropertyName("distinct_size")] int DistinctSize,
    [property: JsonPropertyName("ref")] string Ref,
    [property: JsonPropertyName("ref_type")] string? Ref_Type,
    [property: JsonPropertyName("head")] string Head,
    [property: JsonPropertyName("before")] string Before,
    [property: JsonPropertyName("commits")] IReadOnlyList<Commit> Commits,
    [property: JsonPropertyName("action")] string Action,
    [property: JsonPropertyName("number")] int? Number,
    [property: JsonPropertyName("pull_request")] dynamic PullRequest,
    [property: JsonPropertyName("comment")] Comment Comment,
    [property: JsonPropertyName("review")] dynamic Review,
    [property: JsonPropertyName("issue")] Issue Issue
);
