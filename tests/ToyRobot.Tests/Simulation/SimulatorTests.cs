using FluentAssertions;
using ToyRobot.Commands;
using ToyRobot.Models;
using ToyRobot.Simulation;

namespace ToyRobot.Tests.Simulation;

[Collection("ConsoleOutput")]
public class SimulatorTests
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
    public void Spec_Example1()
    {
        var simulator = new Simulator(new Table());
        var output = CaptureOutput(() =>
        {
            simulator.Execute(new PlaceCommand(0, 0, Direction.North));
            simulator.Execute(new MoveCommand());
            simulator.Execute(new ReportCommand());
        });
        output.Should().Be("0,1,NORTH");
    }

    [Fact]
    public void Spec_Example2()
    {
        var simulator = new Simulator(new Table());
        var output = CaptureOutput(() =>
        {
            simulator.Execute(new PlaceCommand(0, 0, Direction.North));
            simulator.Execute(new LeftCommand());
            simulator.Execute(new ReportCommand());
        });
        output.Should().Be("0,0,WEST");
    }

    [Fact]
    public void Spec_Example3()
    {
        var simulator = new Simulator(new Table());
        var output = CaptureOutput(() =>
        {
            simulator.Execute(new PlaceCommand(1, 2, Direction.East));
            simulator.Execute(new MoveCommand());
            simulator.Execute(new MoveCommand());
            simulator.Execute(new LeftCommand());
            simulator.Execute(new MoveCommand());
            simulator.Execute(new ReportCommand());
        });
        output.Should().Be("3,3,NORTH");
    }

    [Fact]
    public void MoveBeforePlace_Ignored()
    {
        var simulator = new Simulator(new Table());
        var output = CaptureOutput(() =>
        {
            simulator.Execute(new MoveCommand());
            simulator.Execute(new ReportCommand());
            simulator.Execute(new PlaceCommand(0, 0, Direction.North));
            simulator.Execute(new ReportCommand());
        });
        output.Should().Be("0,0,NORTH");
    }

    [Fact]
    public void PlaceOffTable_RobotNotPlaced()
    {
        var simulator = new Simulator(new Table());
        var output = CaptureOutput(() =>
        {
            simulator.Execute(new PlaceCommand(5, 5, Direction.North));
            simulator.Execute(new MoveCommand());
            simulator.Execute(new ReportCommand());
        });
        output.Should().BeEmpty();
    }

    [Fact]
    public void MultiplePlacements_LastWins()
    {
        var simulator = new Simulator(new Table());
        var output = CaptureOutput(() =>
        {
            simulator.Execute(new PlaceCommand(0, 0, Direction.North));
            simulator.Execute(new PlaceCommand(3, 3, Direction.South));
            simulator.Execute(new ReportCommand());
        });
        output.Should().Be("3,3,SOUTH");
    }

    [Fact]
    public void BoundaryProtection_AllEdges()
    {
        var simulator = new Simulator(new Table());
        var output = CaptureOutput(() =>
        {
            simulator.Execute(new PlaceCommand(0, 0, Direction.South));
            simulator.Execute(new MoveCommand());
            simulator.Execute(new ReportCommand());
        });
        output.Should().Be("0,0,SOUTH");
    }
}
