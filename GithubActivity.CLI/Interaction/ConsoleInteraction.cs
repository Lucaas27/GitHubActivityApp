namespace GithubActivity.CLI.Interaction;

public static class ConsoleInteraction
{
    public static void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void DisplayMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{Environment.NewLine}{message}");
        Console.ResetColor();
    }
}