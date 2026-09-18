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
        Load();
    }

    public Contact AddContact(Contact contact)
    {
        contacts.Add(contact);

        Save();
        idCounter++;

        return contact;
    }

    public Contact? GetContact(Guid id)
    {
        return contacts.FirstOrDefault(contact => contact.Id == id);
    }

    public List<Contact> FindByNumber(string number)
    {
        return contacts
            .Where(contact => contact.PhoneNumber == number)
            .ToList();
    }

    public List<Contact> GetContacts()
    {
        return contacts;
    }

    public void Delete(Contact contact)
    {
        contacts.Remove(contact);
        Save();

    }

    public int NewId()
    {
        return idCounter++;
    }

    void Load()
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

    public void Save()
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
