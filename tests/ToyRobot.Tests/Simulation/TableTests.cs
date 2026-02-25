using FluentAssertions;
using Xunit;
using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Tests.Simulation;

public class TableTests
{
    private readonly Table _table = new();

    [Fact]
    public void IsValidPosition_Origin_ReturnsTrue()
    {
        _table.IsValidPosition(new Position(0, 0)).Should().BeTrue();
    }

    [Fact]
    public void IsValidPosition_MaxCorner_ReturnsTrue()
    {
        _table.IsValidPosition(new Position(4, 4)).Should().BeTrue();
    }

    [Fact]
    public void IsValidPosition_Center_ReturnsTrue()
    {
        _table.IsValidPosition(new Position(2, 2)).Should().BeTrue();
    }

    [Fact]
    public void IsValidPosition_XOutOfRange_ReturnsFalse()
    {
        _table.IsValidPosition(new Position(5, 0)).Should().BeFalse();
    }

    [Fact]
    public void IsValidPosition_YOutOfRange_ReturnsFalse()
    {
        _table.IsValidPosition(new Position(0, 5)).Should().BeFalse();
    }

    [Fact]
    public void IsValidPosition_NegativeX_ReturnsFalse()
    {
        _table.IsValidPosition(new Position(-1, 0)).Should().BeFalse();
    }

    [Fact]
    public void IsValidPosition_NegativeY_ReturnsFalse()
    {
        _table.IsValidPosition(new Position(0, -1)).Should().BeFalse();
    }
}
