namespace ContactBook;

static class Helpers
{

    public static void DisplayContacts(List<Contact> contacts)
    {
        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts found.");
            return;
        }

        var idWidth = Math.Max(2, contacts.Max(c => c.Id.ToString().Length));
        var nameWidth = Math.Max(4, contacts.Max(c => c.Name.Length));
        var phoneWidth = Math.Max(5, contacts.Max(c => c.PhoneNumber.Length));

        Console.WriteLine(
            $"{"ID".PadRight(idWidth)}  " +
            $"{"NAME".PadRight(nameWidth)}  " +
            $"{"PHONE".PadRight(phoneWidth)}"
        );

        Console.WriteLine(
            $"{new string('-', idWidth)}  " +
            $"{new string('-', nameWidth)}  " +
            $"{new string('-', phoneWidth)}"
        );

        foreach (var contact in contacts)
        {
            Console.WriteLine(
                $"{contact.Id.ToString().PadRight(idWidth)}  " +
                $"{contact.Name.PadRight(nameWidth)}  " +
                $"{contact.PhoneNumber.PadRight(phoneWidth)}"
            );
        }
    }


    public static bool Confirm(string message)
    {
        Console.Write($"{message} [Y/n]: ");

        var key = Console.ReadKey(intercept: true).Key;

        if (key is ConsoleKey.N or ConsoleKey.Escape)
        {
            Console.WriteLine("n");
            return false;
        }

        Console.WriteLine("y");
        return true;
    }

    public static Dictionary<string, string> ParseOptions(string[] args)
    {
        var options = new Dictionary<string, string>();

        foreach (var arg in args)
        {
            if (!arg.StartsWith("--"))
            {
                continue;
            }

            var parts = arg[2..].Split('=', 2);

            if (parts.Length == 2)
            {
                var key = parts[0];
                var value = parts[1];

                options[key] = value;
            }
        }

        return options;
    }
}
