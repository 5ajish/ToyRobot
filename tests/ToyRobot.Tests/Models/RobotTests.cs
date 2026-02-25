using FluentAssertions;
using ToyRobot.Models;

namespace ToyRobot.Tests.Models;

public class RobotTests
{
    [Fact]
    public void IsPlaced_Initially_ReturnsFalse()
    {
        var robot = new Robot();

        robot.IsPlaced.Should().BeFalse();
    }

    [Fact]
    public void Place_ValidArgs_SetsPositionAndDirection()
    {
        var robot = new Robot();

        robot.Place(1, 2, Direction.East);

        robot.Position.Should().Be(new Position(1, 2));
        robot.Direction.Should().Be(Direction.East);
        robot.IsPlaced.Should().BeTrue();
    }

    [Fact]
    public void NextPosition_FacingNorth_ReturnsOneStepNorth()
    {
        var robot = new Robot();
        robot.Place(0, 0, Direction.North);

        var next = robot.NextPosition();

        next.Should().Be(new Position(0, 1));
    }

    [Fact]
    public void NextPosition_FacingEast_ReturnsOneStepEast()
    {
        var robot = new Robot();
        robot.Place(2, 3, Direction.East);

        var next = robot.NextPosition();

        next.Should().Be(new Position(3, 3));
    }

    [Fact]
    public void MoveTo_NewPosition_UpdatesPosition()
    {
        var robot = new Robot();
        robot.Place(0, 0, Direction.North);

        robot.MoveTo(new Position(0, 1));

        robot.Position.Should().Be(new Position(0, 1));
    }

    [Fact]
    public void TurnLeft_FacingNorth_DirectionBecomesWest()
    {
        var robot = new Robot();
        robot.Place(0, 0, Direction.North);

        robot.TurnLeft();

        robot.Direction.Should().Be(Direction.West);
    }

    [Fact]
    public void TurnRight_FacingNorth_DirectionBecomesEast()
    {
        var robot = new Robot();
        robot.Place(0, 0, Direction.North);

        robot.TurnRight();

        robot.Direction.Should().Be(Direction.East);
    }

    [Fact]
    public void Report_Placed_ReturnsFormattedString()
    {
        var robot = new Robot();
        robot.Place(1, 2, Direction.East);

        robot.Report().Should().Be("1,2,EAST");
    }

    [Fact]
    public void Place_CalledTwice_OverwritesState()
    {
        var robot = new Robot();
        robot.Place(0, 0, Direction.North);

        robot.Place(3, 3, Direction.South);

        robot.Position.Should().Be(new Position(3, 3));
        robot.Direction.Should().Be(Direction.South);
    }
}
