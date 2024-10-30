using System.Net;
using System.Text.Json;
using GithubActivity.App.DTOs;
using GithubActivity.App.Interfaces;
using GithubActivity.App.Models;

namespace GithubActivity.App.Services;

public class GitHubActivityService : IGitHubActivityService
{
    private readonly HttpClient _httpClient = new();
    private const string GitHubApiBaseUrl = "https://api.github.com";

    public GitHubActivityService()
    {
        _httpClient.BaseAddress = new Uri(GitHubApiBaseUrl);
        _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
    }

    public IEnumerable<string> MapEventTypeToAction(IEnumerable<EventModel> events)
    {
        var eventTypesGrouped = GroupEventTypes(events);
        List<string> actions = new();

        foreach (var (type, eventList) in eventTypesGrouped)
        {
            foreach (var @event in eventList)
            {
                var action = GetActionString(@event);
                actions.Add(action);
            }
        }

        return actions;
    }

    private static string GetActionString(EventModel @event)
    {
        var type = @event.Type;
        var refType = @event.Payload?.Ref_Type ?? "unknown";
        var refValue = @event.Payload?.Ref ?? "unknown";
        var comment = @event.Payload?.Comment?.Body ?? "unknown";
        var issueTitle = @event.Payload?.Issue?.Title ?? "unknown";
        var commitsCount = @event.Payload?.Commits?.Count ?? 0;
        var createdAt = $"{@event.CreatedAt:yyyy-MM-dd} at {@event.CreatedAt:HH:mm:ss}";
        var repoName = @event.RepoName;

        return type switch
        {
            "CreateEvent" => $"Created a {refType} {refValue} in {repoName} on {createdAt}",
            "CommitCommentEvent" => $"Commented on a commit in {repoName} on {createdAt}",
            "DeleteEvent" => $"Deleted {repoName} on {createdAt}",
            "ForkEvent" => $"Forked a repository in {repoName} on {createdAt}",
            "GollumEvent" => $"Performed a wiki edit on {createdAt}",
            "IssuesEvent" => $"Opened an issue in {repoName} on {createdAt}",
            "IssueCommentEvent" => $"Commented  \"{comment}\" on the issue \"{issueTitle}\" in {repoName} on {createdAt}",
            "PullRequestEvent" => $"Opened a pull request in {repoName} on {createdAt}",
            "PullRequestReviewEvent" => $"Reviewed a pull request in {repoName} on {createdAt}",
            "PullRequestReviewCommentEvent" => $"Commented on a pull request in {repoName} on {createdAt}",
            "PushEvent" => $"Pushed {commitsCount} commit(s) to {repoName} on {createdAt}",
            "PullRequestReviewThreadEvent" => $"Commented on a pull request review in {repoName} on {createdAt}",
            "SponsorshipEvent" => $"Sponsored a user in {repoName} on {createdAt}",
            "ReleaseEvent" => $"Published a release in {repoName} on {createdAt}",
            "WatchEvent" => $"Starred the repo {repoName} on {createdAt}",
            _ => $"Performed an unknown action in {repoName} on {createdAt}"
        };
    }

    // public IEnumerable<string> MapEventTypeToAction(IEnumerable<EventModel> events)
    // {

    //     var eventTypesMapped = MapEventTypes(events);
    //     List<string> actions = new();

    //     foreach (var (type, eventList) in eventTypesMapped)
    //     {
    //         foreach (var @event in eventList)
    //         {
    //             switch (type)
    //             {
    //                 case "CreateEvent":
    //                     actions.Add($"Created a {@event.Payload.Ref_Type} {@event.Payload.Ref} in {@event.RepoName} on {@event.CreatedAt.ToString().Split(" ")[0]} at {@event.CreatedAt.ToString().Split(" ")[1]}");

    //                     break;
    //                 case "CommitCommentEvent":
    //                     actions.Add($"Commented \"{@event.Payload.Comment}\" on a commit in {@event.RepoName} on {@event.CreatedAt}");

    //                     break;
    //                 case "DeleteEvent":
    //                     actions.Add($"Deleted a repository in {@event.RepoName} on {@event.CreatedAt}");

    //                     break;

    //                 case "ForkEvent":
    //                     actions.Add($"Forked a repository in {@event.RepoName} on {@event.CreatedAt}");

    //                     break;
    //                 case "GollumEvent":
    //                     actions.Add("Performed a wiki edit");

    //                     break;

    //                 case "IssuesEvent":
    //                     actions.Add($"Opened an issue in {@event.RepoName} on {@event.CreatedAt}");

    //                     break;

    //                 case "IssueCommentEvent":
    //                     actions.Add($"Commented on an issue in {@event.RepoName} on {@event.CreatedAt} - {@event.Payload.Issue.Title}");

    //                     break;

    //                 case "PullRequestEvent":
    //                     actions.Add($"Opened a pull request in {@event.RepoName}");

    //                     break;

    //                 case "PullRequestReviewEvent":
    //                     actions.Add($"Reviewed a pull request in {@event.RepoName} on {@event.CreatedAt}");

    //                     break;
    //                 case "PullRequestReviewCommentEvent":
    //                     actions.Add($"Commented on a pull request on {@event.CreatedAt} - {@event.RepoName}");

    //                     break;

    //                 case "PushEvent":
    //                     actions.Add($"Pushed {@event.Payload.Commits.Count} commit(s) to {@event.RepoName} on {@event.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}");

    //                     break;

    //                 case "PullRequestReviewThreadEvent":
    //                     actions.Add($"Commented on a pull request review on {@event.CreatedAt} - {@event.RepoName}");

    //                     break;

    //                 case "SponsorshipEvent":
    //                     actions.Add($"Sponsored a user in {@event.RepoName} on {@event.CreatedAt}");

    //                     break;

    //                 case "ReleaseEvent":
    //                     actions.Add($"Published a release in {@event.RepoName} on {@event.CreatedAt}");

    //                     break;

    //                 case "WatchEvent":
    //                     actions.Add($"Starred the repo {@event.RepoName} on {@event.CreatedAt}");

    //                     break;
    //                 default:
    //                     actions.Add($"Performed an unknown action in {@event.RepoName} on {@event.CreatedAt}");

    //                     break;
    //             }


    //         }

    //     }

    //     return actions;
    // }


    private static Dictionary<string, List<EventModel>> GroupEventTypes(IEnumerable<EventModel> events)
    {
        return events.GroupBy(e => e.Type)
                     .ToDictionary(g => g.Key, g => g.ToList());
    }


    public async Task<IEnumerable<EventModel>> GetRecentActivityAsync(string username)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"users/{username.ToLower()}/events");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var events = JsonSerializer.Deserialize<IEnumerable<EventDTO>>(json);

            var transformedEvents = events?.Select(e => (EventModel)e);

            return transformedEvents ?? Enumerable.Empty<EventModel>();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("User not found. Please check the username and try again.");
            }
            else
            {
                throw new Exception("An error occurred while fetching the user's activity. Please try again.");
            }
        }
    }

}
