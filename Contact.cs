namespace ContactBook;

public class Contact
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string PhoneNumber { get; set; } = "";

    public override string ToString()
    {
        return $"{Name} ({PhoneNumber})";
    }
}
