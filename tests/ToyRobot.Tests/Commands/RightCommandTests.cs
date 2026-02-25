using FluentAssertions;
using Xunit;
using ToyRobot.Commands;
using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Tests.Commands;

public class RightCommandTests
{
    private readonly Table _table = new();
    private readonly RightCommand _command = new();

    [Theory]
    [InlineData(Direction.North, Direction.East)]
    [InlineData(Direction.East, Direction.South)]
    [InlineData(Direction.South, Direction.West)]
    [InlineData(Direction.West, Direction.North)]
    public void Execute_RotatesRight(Direction initial, Direction expected)
    {
        var robot = new Robot();
        robot.Place(0, 0, initial);
        _command.Execute(robot, _table);

        robot.Direction.Should().Be(expected);
    }
}
