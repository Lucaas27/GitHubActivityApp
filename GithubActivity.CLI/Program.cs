using GithubActivity.CLI.Interaction;

var appStartup = new Startup();

try
{
    if (args.Length > 0)
    {
        var username = args[0];
        await appStartup.Run(username);
    }
    else
    {
        Interaction.DisplayMessage("Please provide a username as a command-line argument.");
    }
}
catch (Exception ex)
{
    Interaction.DisplayError($"An error occurred: {ex.Message}");
}
