var width = args.Length > 0 && int.TryParse(args[0], out var w) ? w : 5;
var height = args.Length > 1 && int.TryParse(args[1], out var h) ? h : 5;

var table = new Table(width, height);
var simulator = new Simulator(table);
var parser = new CommandParser();
var interactive = !Console.IsInputRedirected;

if (interactive)
{
    var tableInfo = $"Table: {width}x{height} grid";
    Console.WriteLine("╔════════════════════════════════════════════╗");
    Console.WriteLine("║         TOY ROBOT SIMULATOR                ║");
    Console.WriteLine("╠════════════════════════════════════════════╣");
    Console.WriteLine("║  Commands:                                 ║");
    Console.WriteLine("║    PLACE X,Y,F  - Place robot on table     ║");
    Console.WriteLine("║    MOVE         - Move one step forward    ║");
    Console.WriteLine("║    LEFT         - Rotate 90 left           ║");
    Console.WriteLine("║    RIGHT        - Rotate 90 right          ║");
    Console.WriteLine("║    REPORT       - Show current position    ║");
    Console.WriteLine("║    SHOW         - Display table grid       ║");
    Console.WriteLine("║    EXIT         - Quit simulator           ║");
    Console.WriteLine("╠════════════════════════════════════════════╣");
    Console.WriteLine("║  F = NORTH | EAST | SOUTH | WEST           ║");
    Console.WriteLine($"║  {tableInfo,-42}║");
    Console.WriteLine("║  Origin (0,0) = South-West corner          ║");
    Console.WriteLine("╚════════════════════════════════════════════╝");
    Console.WriteLine();
}

while (Console.ReadLine() is { } input)
{
    if (interactive && input.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
        break;

    if (parser.Parse(input) is { } command)
        simulator.Execute(command);
}
