using System.Net;
using System.Text.Json;
using GithubActivity.App.DTOs;
using GithubActivity.App.Interfaces;
using GithubActivity.App.Models;

namespace GithubActivity.App.Services;

public class GitHubActivityService : IGitHubActivityService
{
    private readonly HttpClient _httpClient;
    private const string GitHubApiBaseUrl = "https://api.github.com";

    public GitHubActivityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(GitHubApiBaseUrl);
        _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
    }

    private string MapEventTypeToAction(EventModel @event)
    {
        var eventType = @event.Type;
        var repoName = @event.RepoName;
        var dateTime = @event.CreatedAt;

        return eventType switch
        {
            "CreateEvent" => $"Created a repository in {repoName} on {dateTime}",
            "CommitCommentEvent" => $"Commented on a commit in {repoName} on {dateTime}",
            "DeleteEvent" => $"Deleted a repository in {repoName} on {dateTime}",
            "ForkEvent" => $"Forked a repository in {repoName} on {dateTime}",
            "GollumEvent" => "Performed a wiki edit",
            "IssuesEvent" => $"Opened an issue in {repoName} on {dateTime}",
            "IssueCommentEvent" => $"Commented on an issue in {repoName} on {dateTime}",
            "PullRequestEvent" => $"Opened a pull request in {repoName}",
            "PullRequestReviewEvent" => $"Reviewed a pull request in {repoName} on {dateTime}",
            "PullRequestReviewCommentEvent" => $"Commented on a pull request on {dateTime} - {repoName}",
            "PushEvent" => $"Pushed to a repository in {repoName} on {dateTime}",
            "PullRequestReviewThreadEvent" => $"Commented on a pull request review on {dateTime} - {repoName}",
            "SponsorshipEvent" => $"Sponsored a user in {repoName} on {dateTime}",
            "ReleaseEvent" => $"Published a release in {repoName} on {dateTime}",
            "WatchEvent" => $"Starred a repository in {repoName} on {dateTime}",
            _ => $"Performed an unknown action in {repoName} on {dateTime}"
        };
    }


    public Dictionary<string, List<string>> MapEventTypes(IEnumerable<EventModel> events)
    {
        Dictionary<string, List<string>> eventTypeMapped = new();

        foreach (var @event in events)
        {
            string eventType = @event.Type;

            if (eventTypeMapped.TryGetValue(eventType, out List<string>? value))
            {
                value.Add(MapEventTypeToAction(@event));
            }
            else
            {
                eventTypeMapped[eventType] = new List<string> { MapEventTypeToAction(@event) };
            }
        }

        return eventTypeMapped;
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
