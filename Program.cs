using ContactBook;

if (args.Length == 0)
{
    Console.WriteLine("Usage: contacts <command>");
    return;
}

var command = args[0].ToLower();

var repository = new ContactRepository();

switch (command)
{
    case "add":
        var name = args[1];
        var phoneNumber = args[2];

        var existingContacts = repository.FindByNumber(phoneNumber);
        switch (existingContacts)
        {
            case { Count: 0 }:
                break;
            case { Count: 1 }:
                if (!Confirm($"Found an existing contact with phone number {phoneNumber}."))
                {
                    Console.WriteLine("Cancelled!");
                    Environment.Exit(0);
                }
                break;
            case { Count: var count } when count > 1:
                if (!Confirm($"Found {count} existing contacts with phone number {phoneNumber}."))
                {
                    Console.WriteLine("Cancelled!");
                    Environment.Exit(0);
                }
                break;
            default:
                break;
        }

        Console.WriteLine($"Adding {name}: {phoneNumber}");

        var contact = new Contact { Name = name, PhoneNumber = phoneNumber };
        repository.AddContact(contact);

        break;
    case "list":
        var contacts = repository.GetContacts();
        DisplayContacts(contacts);

        break;

    case "delete":
        var contactName = args[1];

        Console.WriteLine($"Deleting {contactName}...");
        break;
    default:
        Console.WriteLine($"Unknown command: {command}");
        break;
}

static void DisplayContacts(List<Contact> contacts)
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


static bool Confirm(string message)
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
