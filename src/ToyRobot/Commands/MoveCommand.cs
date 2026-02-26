namespace ToyRobot.Commands;

public class MoveCommand : ICommand
{
    public void Execute(Robot robot, Table table)
    {
        var newPosition = robot.NextPosition();
        if (table.IsValidPosition(newPosition))
            robot.MoveTo(newPosition);
    }
}
