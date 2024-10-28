using GithubActivity.App.DTOs;

namespace GithubActivity.App.Models;

public record EventModel(
    string Id,
    string Type,
    string DisplayLogin,
    string RepoName,
    bool Public,
    DateTime CreatedAt
)
{
    // public string MapEventTypeToAction()
    // {
    //     return Type switch
    //     {
    //         "CreateEvent" => $"Created a repository at {RepoName}",
    //         "CommitCommentEvent" => "Commented on a commit",
    //         "DeleteEvent" => "Deleted a repository",
    //         "ForkEvent" => $"Forked a repository at {RepoName}",
    //         "GollumEvent" => "Performed a wiki edit",
    //         "IssuesEvent" => "Opened an issue",
    //         "IssueCommentEvent" => "Commented on an issue",
    //         "PullRequestEvent" => "Opened a pull request",
    //         "PullRequestReviewEvent" => "Reviewed a pull request",
    //         "PullRequestReviewCommentEvent" => "Commented on a pull request",
    //         "PushEvent" => "Pushed to a repository",
    //         "PullRequestReviewThreadEvent" => "Commented on a pull request review",
    //         "SponsorshipEvent" => "Sponsored a user",
    //         "ReleaseEvent" => "Published a release",
    //         "WatchEvent" => "Starred a repository",
    //         _ => "Performed an unknown action"
    //     };
    // }


    public static explicit operator EventModel(EventDTO dto)
    {

        return new EventModel(
            dto.Id,
            dto.Type,
            dto.Actor.DisplayLogin,
            dto.Repo.Name,
            dto.Public,
            dto.CreatedAt
        );

    }

}