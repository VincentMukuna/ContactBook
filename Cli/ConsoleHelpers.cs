namespace ContactBook.Cli;

public static class ConsoleHelpers
{
    public static bool Confirm(string message)
    {
        Console.Write($"{message} [Y/n]: ");

        var key = Console.ReadKey(intercept: true).Key;

        Console.WriteLine();

        return key is not ConsoleKey.N and not ConsoleKey.Escape;
    }

    public static void Error(string message)
    {
        Console.Error.WriteLine($"Error: {message}");
    }
}
