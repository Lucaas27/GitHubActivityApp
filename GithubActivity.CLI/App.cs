using GithubActivity.App.Interfaces;
using GithubActivity.App.Models;
using GithubActivity.CLI.Interaction;

namespace GithubActivity.CLI;

public class App
{

    private readonly IGitHubActivityService _gitHubActivityService;

    public App(IGitHubActivityService gitHubActivityService)
    {
        _gitHubActivityService = gitHubActivityService;
    }

    private void GetEventsDetails(IEnumerable<EventModel> events)
    {
        var actions = _gitHubActivityService.MapEventTypeToAction(events);
        ConsoleInteraction.DisplayMessage("Recent activity:");
        foreach (var action in actions
                                    .OrderBy(a => a.Split(" ")[^2]) // Primary sort by the date
                                    .ThenBy(a => a.Split(" ")[^1])) // Secondary sort by the time
        {
            ConsoleInteraction.DisplayMessage(action);
        }


    }

    public async Task Run(string username)
    {
        var events = await _gitHubActivityService.GetRecentActivityAsync(username);

        if (events is null || !events.Any())
        {
            ConsoleInteraction.DisplayMessage("No activity found for the specified user in the last 90 days.");
            return;
        }

        GetEventsDetails(events);

    }
}