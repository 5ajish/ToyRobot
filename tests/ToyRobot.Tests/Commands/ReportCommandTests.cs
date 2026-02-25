using FluentAssertions;
using Xunit;
using ToyRobot.Commands;
using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Tests.Commands;

public class ReportCommandTests
{
    [Fact]
    public void Execute_PlacedRobot_OutputsPosition()
    {
        var robot = new Robot();
        robot.Place(1, 2, Direction.East);
        var table = new Table();
        var command = new ReportCommand();

        var sw = new StringWriter();
        Console.SetOut(sw);

        command.Execute(robot, table);

        sw.ToString().Trim().Should().Be("1,2,EAST");
    }
}
