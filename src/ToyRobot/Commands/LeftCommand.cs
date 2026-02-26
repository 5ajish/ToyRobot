namespace ToyRobot.Commands;

public class LeftCommand : ICommand
{
    public void Execute(Robot robot, Table table) => robot.TurnLeft();
}
