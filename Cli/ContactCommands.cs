namespace ContactBook.Cli;

public class ContactCommands
{
    private readonly ContactRepository repository;

    public ContactCommands(ContactRepository repository)
    {
        this.repository = repository;
    }

    public void List(string[] args)
    {
        var contacts = repository.GetContacts();

        Console.WriteLine();
        ContactPrinter.Display(contacts);
        Console.WriteLine();
    }

    public void Add(string[] args)
    {
        var name = args[1];
        var phoneNumber = args[2];

        var existingContacts = repository.FindByNumber(phoneNumber);

        if (existingContacts.Count > 0)
        {
            Console.WriteLine(
                $"Found {existingContacts.Count} existing contacts with phone number {phoneNumber}:\n"
            );

            ContactPrinter.Display(existingContacts);

            if (!ConsoleHelpers.Confirm("\nAdd a new entry?"))
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

    public void Update(string[] args)
    {
        var id = args[1];

        var guid = Guid.Parse(id);

        var contact = repository.GetContact(guid);

        if (contact is null)
        {
            Console.Error.WriteLine($"No contact found with id {id}");
            return;
        }

        var options = ArgumentParser.ParseOptions(args[2..]);

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

    public void Delete(string[] args)
    {
        var phoneNumber = args[1];

        var matches = repository.FindByNumber(phoneNumber);

        if (matches.Count > 0)
        {
            Console.WriteLine($"Found {matches.Count} contact(s) with phone number {phoneNumber}: \n");
            ContactPrinter.Display(matches);

            if (!ConsoleHelpers.Confirm("\nDelete all contact(s)"))
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

}
