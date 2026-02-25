using FluentAssertions;
using ToyRobot.Models;

namespace ToyRobot.Tests.Models;

public class DirectionExtensionsTests
{
    [Fact]
    public void TurnLeft_FromNorth_ReturnsWest()
    {
        Direction.North.TurnLeft().Should().Be(Direction.West);
    }

    [Fact]
    public void TurnLeft_FromWest_ReturnsSouth()
    {
        Direction.West.TurnLeft().Should().Be(Direction.South);
    }

    [Fact]
    public void TurnLeft_FromSouth_ReturnsEast()
    {
        Direction.South.TurnLeft().Should().Be(Direction.East);
    }

    [Fact]
    public void TurnLeft_FromEast_ReturnsNorth()
    {
        Direction.East.TurnLeft().Should().Be(Direction.North);
    }

    [Fact]
    public void TurnRight_FromNorth_ReturnsEast()
    {
        Direction.North.TurnRight().Should().Be(Direction.East);
    }

    [Fact]
    public void TurnRight_FromEast_ReturnsSouth()
    {
        Direction.East.TurnRight().Should().Be(Direction.South);
    }

    [Fact]
    public void TurnRight_FromSouth_ReturnsWest()
    {
        Direction.South.TurnRight().Should().Be(Direction.West);
    }

    [Fact]
    public void TurnRight_FromWest_ReturnsNorth()
    {
        Direction.West.TurnRight().Should().Be(Direction.North);
    }

    [Fact]
    public void ToMovementVector_North_Returns0_1()
    {
        Direction.North.ToMovementVector().Should().Be(new Position(0, 1));
    }

    [Fact]
    public void ToMovementVector_East_Returns1_0()
    {
        Direction.East.ToMovementVector().Should().Be(new Position(1, 0));
    }

    [Fact]
    public void ToMovementVector_South_Returns0_Neg1()
    {
        Direction.South.ToMovementVector().Should().Be(new Position(0, -1));
    }

    [Fact]
    public void ToMovementVector_West_ReturnsNeg1_0()
    {
        Direction.West.ToMovementVector().Should().Be(new Position(-1, 0));
    }
}
