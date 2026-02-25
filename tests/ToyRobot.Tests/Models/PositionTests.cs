using FluentAssertions;
using ToyRobot.Models;

namespace ToyRobot.Tests.Models;

public class PositionTests
{
    [Fact]
    public void Equality_SameValues_AreEqual()
    {
        var a = new Position(1, 2);
        var b = new Position(1, 2);

        (a == b).Should().BeTrue();
    }

    [Fact]
    public void Equality_DifferentValues_AreNotEqual()
    {
        var a = new Position(1, 2);
        var b = new Position(3, 4);

        (a == b).Should().BeFalse();
    }

    [Fact]
    public void ToString_ReturnsCommaSeparated()
    {
        var position = new Position(3, 4);

        position.ToString().Should().Be("3,4");
    }
}
