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
    private static readonly Dictionary<string, string> _actionTemplates = new()
    {
        { "CreateBranchEvent", "{user} created the branch {refValue} in {repoName} on {createdAt}" },
        { "CreateRepositoryEvent", "{user} created a new repository in {repoName} on {createdAt}" },
        { "CommitCommentEvent", "{user} commented on a commit in {prCommentUrl} on {createdAt}" },
        { "DeleteEvent", "{user} deleted {repoType} {refValue} from {repoName} on {createdAt}" },
        { "ForkEvent", "{user} forked a repository in {repoName} on {createdAt}" },
        { "GollumEvent", "{user} performed a wiki edit on {createdAt}" },
        { "IssuesEvent", "{user} opened the issue \"{issueTitle}\" in {repoName} on {createdAt}" },
        { "IssueCommentEvent", "{user} commented on the issue \"{issueTitle}\" in {prCommentUrl} on {createdAt}" },
        { "PullRequestEvent", "{user} opened a pull request in {repoName} on {createdAt}" },
        { "PullRequestReviewEvent", "{user} reviewed a pull request in {repoName} on {createdAt}" },
        { "PullRequestReviewCommentEvent", "{user} commented on a pull request in {prCommentUrl} on {createdAt}" },
        { "PushEvent", "{user} pushed {commitsCount} commit(s) to {repoName} on branch {refValue} on {createdAt}" },
        { "PullRequestReviewThreadEvent", "{user} commented on a pull request review in {prCommentUrl} on {createdAt}" },
        { "SponsorshipEvent", "{user} sponsored a user in {repoName} on {createdAt}" },
        { "ReleaseEvent", "{user} published a release in {repoName} on {createdAt}" },
        { "WatchEvent", "{user} starred the repo {repoName} on {createdAt}" },
        { "default", "{user} performed an unknown action in {repoName} on {createdAt}" }
    };

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
        var issueTitle = @event.Payload?.Issue?.Title ?? "unknown";
        var commitsCount = @event.Payload?.Commits?.Count ?? 0;
        var createdAt = $"{@event.CreatedAt:dd-MM-yyyy} at {@event.CreatedAt:HH:mm:ss}";
        var repoName = @event.RepoName;
        var user = char.ToUpper(@event.DisplayLogin[0]) + @event.DisplayLogin.Substring(1);
        var prCommentUrl = @event.Payload?.Comment?.HtmlUrl ?? "unknown";


        var template = type == "CreateEvent"
            ? (refType == "repository" ? _actionTemplates["CreateRepositoryEvent"] : _actionTemplates["CreateBranchEvent"])
            : (_actionTemplates.ContainsKey(type) ? _actionTemplates[type] : _actionTemplates["default"]);

        var values = new Dictionary<string, string>
        {
            { "refType", refType },
            { "refValue", refValue },
            { "repoName", repoName },
            { "createdAt", createdAt },
            { "issueTitle", issueTitle },
            { "commitsCount", commitsCount.ToString() },
            { "user", user },
            { "prCommentUrl", prCommentUrl }
        };


        foreach (var kvp in values)
        {
            template = template.Replace($"{{{kvp.Key}}}", kvp.Value);
        }

        return template;

    }


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
