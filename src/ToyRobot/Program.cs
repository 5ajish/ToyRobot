var table = new Table();
var simulator = new Simulator(table);
var parser = new CommandParser();
var interactive = !Console.IsInputRedirected;

if (interactive)
    Console.WriteLine("Toy Robot Simulator - Type commands or EXIT to quit");

while (Console.ReadLine() is { } input)
{
    if (interactive && input.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
        break;

    if (parser.Parse(input) is { } command)
        simulator.Execute(command);
}
