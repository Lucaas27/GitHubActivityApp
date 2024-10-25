using System.Net.Http.Headers;

namespace GitHubApiClient;

/// <summary>
/// A client for interacting with the GitHub API to retrieve user activity.
/// </summary>
public class GitHubApi
{
    private readonly HttpClient _httpClient;
    private const string GitHubApiBaseUrl = "https://api.github.com";

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send requests.</param>
    public GitHubApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(GitHubApiBaseUrl);
        _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
    }

    /// <summary>
    /// Gets the recent activity for a specified GitHub user.
    /// </summary>
    /// <param name="username">The GitHub username.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a string representing the user's recent activity.</returns>
    public async Task<string> GetRecentActivityAsync(string username)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"users/{username}/events");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync() ?? string.Empty;

            return content;
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("An error occurred while retrieving the user's recent activity.", ex);
        }
    }
}
