using GithubActivity.App.Models;

namespace GithubActivity.App.Interfaces;

public interface IGitHubActivityService
{
    Task<IEnumerable<EventModel>> GetRecentActivityAsync(string username);
    IEnumerable<string> MapEventTypeToAction(IEnumerable<EventModel> events);
}