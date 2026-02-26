using ToyRobot.Commands;
using ToyRobot.Models;

namespace ToyRobot.Parsing;

public class CommandParser
{
    public ICommand? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var parts = input.Trim().ToUpperInvariant().Split(' ', 2);

        return parts[0] switch
        {
            "MOVE"   => new MoveCommand(),
            "LEFT"   => new LeftCommand(),
            "RIGHT"  => new RightCommand(),
            "REPORT" => new ReportCommand(),
            "PLACE"  => ParsePlace(parts.Length > 1 ? parts[1] : null),
            _        => null
        };
    }

    private static PlaceCommand? ParsePlace(string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;

        var parts = args.Split(',');
        if (parts.Length != 3)
            return null;

        if (!int.TryParse(parts[0].Trim(), out var x) ||
            !int.TryParse(parts[1].Trim(), out var y) ||
            !Enum.TryParse<Direction>(parts[2].Trim(), ignoreCase: true, out var direction))
            return null;

        return new PlaceCommand(x, y, direction);
    }
}
