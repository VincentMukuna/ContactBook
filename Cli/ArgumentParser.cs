namespace ContactBook.Cli;

public static class ArgumentParser
{
    public static Dictionary<string, string> ParseOptions(string[] args)
    {
        var options = new Dictionary<string, string>();

        foreach (var arg in args)
        {
            if (!arg.StartsWith("--"))
                continue;

            var parts = arg[2..].Split('=', 2);

            if (parts.Length == 2)
            {
                options[parts[0]] = parts[1];
            }
        }

        return options;
    }
}
