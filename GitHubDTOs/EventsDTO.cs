using System.Text.Json.Serialization;

namespace GitHubDTOs;

public record EventsDTO(
    [property: JsonPropertyName("id")] string id,
    [property: JsonPropertyName("type")] string type,
    [property: JsonPropertyName("actor")] Actor actor,
    [property: JsonPropertyName("repo")] Repo repo,
    [property: JsonPropertyName("payload")] Payload payload,
    [property: JsonPropertyName("public")] bool @public,
    [property: JsonPropertyName("created_at")] DateTime created_at
);

public record Actor(
    [property: JsonPropertyName("id")] int id,
    [property: JsonPropertyName("login")] string login,
    [property: JsonPropertyName("display_login")] string display_login,
    [property: JsonPropertyName("gravatar_id")] string gravatar_id,
    [property: JsonPropertyName("url")] string url,
    [property: JsonPropertyName("avatar_url")] string avatar_url
);

public record Author(
    [property: JsonPropertyName("email")] string email,
    [property: JsonPropertyName("name")] string name
);

public record Commit(
    [property: JsonPropertyName("sha")] string sha,
    [property: JsonPropertyName("author")] Author author,
    [property: JsonPropertyName("message")] string message,
    [property: JsonPropertyName("distinct")] bool distinct,
    [property: JsonPropertyName("url")] string url
);

public record Payload(
    [property: JsonPropertyName("repository_id")] int repository_id,
    [property: JsonPropertyName("push_id")] long push_id,
    [property: JsonPropertyName("size")] int size,
    [property: JsonPropertyName("distinct_size")] int distinct_size,
    [property: JsonPropertyName("ref")] string @ref,
    [property: JsonPropertyName("head")] string head,
    [property: JsonPropertyName("before")] string before,
    [property: JsonPropertyName("commits")] IReadOnlyList<Commit> commits
);

public record Repo(
    [property: JsonPropertyName("id")] int id,
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("url")] string url
);


