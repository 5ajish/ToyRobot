namespace ToyRobot.Commands;

public class ReportCommand : ICommand
{
    public void Execute(Robot robot, Table table)
    {
        Console.WriteLine(robot.Report());
    }
}
