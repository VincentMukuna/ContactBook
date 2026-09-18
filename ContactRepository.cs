using System.Text.Json;

namespace ContactBook;

public class ContactRepository
{
    List<Contact> contacts = [];
    readonly string filePath = "data/contacts.json";
    readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };

    int idCounter = 0;

    public ContactRepository()
    {
        EnsureDataDirectory();
        Hydrate();
    }

    public void AddContact(Contact contact)
    {
        contacts.Add(contact);
        Dehydrate();
        idCounter++;
    }

    public  List<Contact> FindByNumber(string number)
    {
        return contacts
            .Where(contact => contact.PhoneNumber == number)
            .ToList();
    }

    public  List<Contact> GetContacts()
    {
        return contacts;
    }

    public int NewId()
    {
        return idCounter++;
    }

    void Hydrate()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            contacts = JsonSerializer.Deserialize<List<Contact>>(json) ?? [];
        }
        else
        {
            contacts = [];
        }

        idCounter = contacts.Count;

    }

    void Dehydrate()
    {
        var json = JsonSerializer.Serialize(
            contacts,
            jsonOptions
        );

        File.WriteAllText(filePath, json);
    }

     void EnsureDataDirectory()
    {
        var directory = Path.GetDirectoryName(filePath);
        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }
    }
}
