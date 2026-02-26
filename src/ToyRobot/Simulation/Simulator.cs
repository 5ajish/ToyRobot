namespace ToyRobot.Simulation;

public class Simulator(Table table)
{
    private readonly Robot _robot = new();

    public void Execute(ICommand command)
    {
        if (!_robot.IsPlaced && command is not PlaceCommand)
            return;

        command.Execute(_robot, table);
    }
}
