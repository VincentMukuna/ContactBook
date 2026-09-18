namespace ContactBook.Cli;

public static class ContactPrinter
{
    public static void Display(IEnumerable<Contact> contacts)
    {
        var contactList = contacts.ToList();

        if (contactList.Count == 0)
        {
            Console.WriteLine("No contacts found.");
            return;
        }

        var idWidth = Math.Max(2, contactList.Max(c => c.Id.ToString().Length));
        var nameWidth = Math.Max(4, contactList.Max(c => c.Name.Length));
        var phoneWidth = Math.Max(5, contactList.Max(c => c.PhoneNumber.Length));

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

        foreach (var contact in contactList)
        {
            Console.WriteLine(
                $"{contact.Id.ToString().PadRight(idWidth)}  " +
                $"{contact.Name.PadRight(nameWidth)}  " +
                $"{contact.PhoneNumber.PadRight(phoneWidth)}"
            );
        }
    }
}
