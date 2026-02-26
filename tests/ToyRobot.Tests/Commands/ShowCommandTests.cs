using FluentAssertions;
using ToyRobot.Commands;
using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Tests.Commands;

[Collection("ConsoleOutput")]
public class ShowCommandTests
{
    private static string CaptureOutput(Action action)
    {
        var sw = new StringWriter();
        Console.SetOut(sw);
        action();
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        return sw.ToString().Trim();
    }

    [Fact]
    public void Execute_NotPlaced_NoOutput()
    {
        var robot = new Robot();
        var table = new Table();
        var command = new ShowCommand();

        var output = CaptureOutput(() => command.Execute(robot, table));

        output.Should().BeEmpty();
    }

    [Fact]
    public void Execute_PlacedAtOriginNorth_ShowsArrowAtOrigin()
    {
        var robot = new Robot();
        robot.Place(0, 0, Direction.North);
        var table = new Table();
        var command = new ShowCommand();

        var output = CaptureOutput(() => command.Execute(robot, table));

        output.Should().Contain("▲");
        output.Should().Contain("0   1   2   3   4");
    }

    [Fact]
    public void Execute_PlacedFacingEast_ShowsRightArrow()
    {
        var robot = new Robot();
        robot.Place(2, 3, Direction.East);
        var table = new Table();
        var command = new ShowCommand();

        var output = CaptureOutput(() => command.Execute(robot, table));

        output.Should().Contain("►");
    }

    [Fact]
    public void Execute_PlacedFacingSouth_ShowsDownArrow()
    {
        var robot = new Robot();
        robot.Place(4, 4, Direction.South);
        var table = new Table();
        var command = new ShowCommand();

        var output = CaptureOutput(() => command.Execute(robot, table));

        output.Should().Contain("▼");
    }

    [Fact]
    public void Execute_PlacedFacingWest_ShowsLeftArrow()
    {
        var robot = new Robot();
        robot.Place(1, 1, Direction.West);
        var table = new Table();
        var command = new ShowCommand();

        var output = CaptureOutput(() => command.Execute(robot, table));

        output.Should().Contain("◄");
    }
}
