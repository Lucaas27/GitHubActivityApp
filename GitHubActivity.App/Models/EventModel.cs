using GithubActivity.App.DTOs;

namespace GithubActivity.App.Models;

public record EventModel(
    string Id,
    string Type,
    string DisplayLogin,
    string RepoName,
    bool Public,
    DateTime CreatedAt,
    Payload Payload
)
{

    public static explicit operator EventModel(EventDTO dto)
    {

        return new EventModel(
            dto.Id,
            dto.Type,
            dto.Actor.DisplayLogin,
            dto.Repo.Name,
            dto.Public,
            dto.CreatedAt,
            dto.Payload
        );

    }

}