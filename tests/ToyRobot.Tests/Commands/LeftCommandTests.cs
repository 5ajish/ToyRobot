using FluentAssertions;
using Xunit;
using ToyRobot.Commands;
using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Tests.Commands;

public class LeftCommandTests
{
    private readonly Table _table = new();
    private readonly LeftCommand _command = new();

    [Theory]
    [InlineData(Direction.North, Direction.West)]
    [InlineData(Direction.West, Direction.South)]
    [InlineData(Direction.South, Direction.East)]
    [InlineData(Direction.East, Direction.North)]
    public void Execute_RotatesLeft(Direction initial, Direction expected)
    {
        var robot = new Robot();
        robot.Place(0, 0, initial);
        _command.Execute(robot, _table);

        robot.Direction.Should().Be(expected);
    }
}
