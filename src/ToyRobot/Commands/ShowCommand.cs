namespace ToyRobot.Commands;

public class ShowCommand : ICommand
{
    private static readonly Dictionary<Direction, string> Arrows = new()
    {
        [Direction.North] = "▲",
        [Direction.East]  = "►",
        [Direction.South] = "▼",
        [Direction.West]  = "◄"
    };

    public void Execute(Robot robot, Table table)
    {
        if (!robot.IsPlaced) return;

        var pos = robot.Position!;
        var dir = robot.Direction!.Value;
        var w = table.Width;
        var h = table.Height;

        var info = new[]
        {
            $"  Position : ({pos.X},{pos.Y})",
            $"  Facing   : {dir.ToString().ToUpperInvariant()}",
            "",
            "      N",
            "      |",
            "  W --+-- E",
            "      |",
            "      S"
        };

        Console.WriteLine("  ┌" + string.Join("┬", Enumerable.Repeat("───", w)) + "┐");
        var infoLine = 0;
        for (var y = h - 1; y >= 0; y--)
        {
            Console.Write($"{y} │");
            for (var x = 0; x < w; x++)
            {
                if (x == pos.X && y == pos.Y)
                    Console.Write($" {Arrows[dir]} │");
                else
                    Console.Write("   │");
            }
            Console.WriteLine(infoLine < info.Length ? $"  {info[infoLine++]}" : "");
            if (y > 0)
            {
                Console.Write("  ├" + string.Join("┼", Enumerable.Repeat("───", w)) + "┤");
                Console.WriteLine(infoLine < info.Length ? $"  {info[infoLine++]}" : "");
            }
        }
        Console.WriteLine("  └" + string.Join("┴", Enumerable.Repeat("───", w)) + "┘");
        Console.WriteLine("    " + string.Join("   ", Enumerable.Range(0, w)));
    }
}
