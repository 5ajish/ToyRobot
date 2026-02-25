using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Commands;

public interface ICommand
{
    void Execute(Robot robot, Table table);
}
