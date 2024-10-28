using GithubActivity.App.Models;

namespace GithubActivity.App.Interfaces;

public interface IGitHubActivityService
{
    Task<IEnumerable<EventModel>> GetRecentActivityAsync(string username);
}