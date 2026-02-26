namespace ToyRobot.Simulation;

public class Table(int width = 5, int height = 5)
{
    public bool IsValidPosition(Position pos) =>
        pos.X >= 0 && pos.X < width && pos.Y >= 0 && pos.Y < height;
}
