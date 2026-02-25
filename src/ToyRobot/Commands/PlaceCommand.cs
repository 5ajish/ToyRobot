using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Commands;

public class PlaceCommand(int x, int y, Direction direction) : ICommand
{
    public void Execute(Robot robot, Table table)
    {
        if (table.IsValidPosition(new Position(x, y)))
            robot.Place(x, y, direction);
    }
}
