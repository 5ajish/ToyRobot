namespace ToyRobot.Models;

public record Position(int X, int Y)
{
    public override string ToString() => $"{X},{Y}";
}
