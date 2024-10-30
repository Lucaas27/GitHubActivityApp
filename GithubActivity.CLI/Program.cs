using GithubActivity.App.Services;
using GithubActivity.CLI;
using GithubActivity.CLI.Interaction;

var gitHubActivityService = new GitHubActivityService();
var app = new App(gitHubActivityService);

try
{
    if (args.Length > 0)
    {
        var username = args[0];
        await app.Run(username);
    }
    else
    {
        ConsoleInteraction.DisplayMessage("Please provide a github username as a command-line argument.");
    }
}
catch (Exception ex)
{
    ConsoleInteraction.DisplayError($"An error occurred: {ex.Message}");
    ConsoleInteraction.DisplayError($"{ex.StackTrace}");
}
