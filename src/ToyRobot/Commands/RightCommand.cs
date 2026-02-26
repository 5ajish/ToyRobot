namespace ToyRobot.Commands;

public class RightCommand : ICommand
{
    public void Execute(Robot robot, Table table) => robot.TurnRight();
}
