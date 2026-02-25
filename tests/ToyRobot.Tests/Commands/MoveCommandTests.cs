using FluentAssertions;
using Xunit;
using ToyRobot.Commands;
using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Tests.Commands;

public class MoveCommandTests
{
    private readonly Robot _robot = new();
    private readonly Table _table = new();
    private readonly MoveCommand _command = new();

    [Fact]
    public void Execute_ValidMove_RobotMoves()
    {
        _robot.Place(0, 0, Direction.North);
        _command.Execute(_robot, _table);

        _robot.Position.Should().Be(new Position(0, 1));
    }

    [Fact]
    public void Execute_WouldFallNorth_RobotStays()
    {
        _robot.Place(0, 4, Direction.North);
        _command.Execute(_robot, _table);

        _robot.Position.Should().Be(new Position(0, 4));
    }

    [Fact]
    public void Execute_WouldFallEast_RobotStays()
    {
        _robot.Place(4, 0, Direction.East);
        _command.Execute(_robot, _table);

        _robot.Position.Should().Be(new Position(4, 0));
    }

    [Fact]
    public void Execute_WouldFallSouth_RobotStays()
    {
        _robot.Place(0, 0, Direction.South);
        _command.Execute(_robot, _table);

        _robot.Position.Should().Be(new Position(0, 0));
    }

    [Fact]
    public void Execute_WouldFallWest_RobotStays()
    {
        _robot.Place(0, 0, Direction.West);
        _command.Execute(_robot, _table);

        _robot.Position.Should().Be(new Position(0, 0));
    }
}
