using ContactBook;
using ContactBook.Cli;


if (args.Length == 0)
{
    Console.WriteLine("Usage: contacts <command>");
    return;
}

var command = args[0].ToLower();

var repository = new ContactRepository();
var commands = new ContactCommands(repository);

switch (command)
{
    case "add":
        commands.Add(args);
        break;

    case "update":
        commands.Update(args);
        break;

    case "list":
        commands.List(args);
        break;

    case "delete":
        commands.Delete(args);
        break;


    default:
        Console.WriteLine($"Unknown command: {command}");
        break;
}
