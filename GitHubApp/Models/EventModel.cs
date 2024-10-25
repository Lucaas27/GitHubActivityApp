using GitHubApp.DTOs;

namespace GitHubApp.Models;

public record EventModel(
    string Id,
    string Type,
    string ActorLogin,
    bool Public,
    DateTime CreatedAt
)
{
    public static explicit operator EventModel(EventDTO dto) => new(
        dto.Id,
        dto.Type,
        dto.Actor.Login,
        dto.Public,
        dto.CreatedAt
    );
}