using FluentAssertions;
using Xunit;
using ToyRobot.Commands;
using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Tests.Commands;

public class PlaceCommandTests
{
    private readonly Robot _robot = new();
    private readonly Table _table = new();

    [Fact]
    public void Execute_ValidPosition_PlacesRobot()
    {
        var command = new PlaceCommand(0, 0, Direction.North);
        command.Execute(_robot, _table);

        _robot.IsPlaced.Should().BeTrue();
        _robot.Position.Should().Be(new Position(0, 0));
        _robot.Direction.Should().Be(Direction.North);
    }

    [Fact]
    public void Execute_OffTable_DoesNotPlace()
    {
        var command = new PlaceCommand(5, 5, Direction.North);
        command.Execute(_robot, _table);

        _robot.IsPlaced.Should().BeFalse();
    }

    [Fact]
    public void Execute_NegativeCoords_DoesNotPlace()
    {
        var command = new PlaceCommand(-1, 0, Direction.North);
        command.Execute(_robot, _table);

        _robot.IsPlaced.Should().BeFalse();
    }

    [Fact]
    public void Execute_EdgePosition_PlacesRobot()
    {
        var command = new PlaceCommand(4, 4, Direction.South);
        command.Execute(_robot, _table);

        _robot.IsPlaced.Should().BeTrue();
        _robot.Position.Should().Be(new Position(4, 4));
        _robot.Direction.Should().Be(Direction.South);
    }
}
