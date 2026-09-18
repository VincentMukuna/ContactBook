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
        AddContact(args);
        break;

    case "update":
        UpdateContact(args);
        break;

    case "list":
        var contacts = repository.GetContacts();
        DisplayContacts(contacts);
        break;

    case "delete":
        DeleteContact(args);
        break;


    default:
        Console.WriteLine($"Unknown command: {command}");
        break;
}

void AddContact(string[] args)
{
    var name = args[1];
    var phoneNumber = args[2];

    var existingContacts = repository.FindByNumber(phoneNumber);

    if (existingContacts.Count > 0 )
    {
        Console.WriteLine($"Found {existingContacts.Count} existing contacts with phone number {phoneNumber}: \n");
        DisplayContacts(existingContacts);
        if(!Confirm("\nAdd a new entry?"))
        {
            Console.WriteLine("Cancelled!");
            return;
        }
    }

    var contact = repository.AddContact(new Contact
    {
        Name = name,
        PhoneNumber = phoneNumber
    });

    Console.WriteLine($"Added {contact.Id} {contact}");
}

void UpdateContact(string[] args)
{
    var id = args[1];

    var guid = Guid.Parse(id);

    var contact = repository.GetContact(guid);

    if (contact is null)
    {
        Console.Error.WriteLine($"No contact found with id {id}");
        return;
    }

    var options = ParseOptions(args[2..]);

    if (options.TryGetValue("name", out var name))
    {
        contact.Name = name;
    }

    if (options.TryGetValue("phone", out var phone))
    {
        contact.PhoneNumber = phone;
    }

    repository.Save();

    Console.WriteLine($"Updated {contact}");
}

void DeleteContact(string[] args)
{
    var phoneNumber = args[1];

    var matches = repository.FindByNumber(phoneNumber);

    if (matches.Count > 0)
    {
        Console.WriteLine($"Found {matches.Count} contact(s) with phone number {phoneNumber}: \n");
        DisplayContacts(matches);

        if (!Confirm("\nDelete all contact(s)"))
        {
            Console.WriteLine("Cancelled!");
            return;
        }

    }

    foreach (var contact in matches)
    {
        repository.Delete(contact);
    }

    Console.WriteLine($"Deleting...");
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

static Dictionary<string, string> ParseOptions(string[] args)
{
    var options = new Dictionary<string, string>();

    foreach (var arg in args)
    {
        if (!arg.StartsWith("--"))
        {
            continue;
        }

        var parts = arg[2..].Split('=', 2);

        if(parts.Length == 2)
        {
            var key = parts[0];
            var value = parts[1];

            options[key] = value;
        }
    }

    return options;
}
