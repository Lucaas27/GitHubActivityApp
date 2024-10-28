using GithubActivity.App.Services;
using GithubActivity.CLI.Interaction;
public class Startup
{
    public async Task Run(string username)
    {
        var httpClient = new HttpClient();
        var gitHubActivityService = new GitHubActivityService(httpClient);
        var events = await gitHubActivityService.GetRecentActivityAsync(username);

        if (events is null || !events.Any())
        {
            Interaction.DisplayError("No activity found for the specified user.");
            return;
        }

        var eventTypesMapped = gitHubActivityService.MapEventTypes(events);

        foreach (var (eventType, eventList) in eventTypesMapped)
        {
            Interaction.DisplayMessage($"{eventList.Count} events of type {eventType}:{Environment.NewLine}");
            foreach (var @event in eventList)
            {
                Interaction.DisplayMessage($"- {@event}{Environment.NewLine}");
            }
        }
    }
}