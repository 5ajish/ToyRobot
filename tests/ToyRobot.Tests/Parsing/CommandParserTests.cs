using FluentAssertions;
using ToyRobot.Commands;
using ToyRobot.Parsing;

namespace ToyRobot.Tests.Parsing;

public class CommandParserTests
{
    private readonly CommandParser _parser = new();

    [Fact]
    public void Parse_Null_ReturnsNull()
    {
        _parser.Parse(null).Should().BeNull();
    }

    [Fact]
    public void Parse_Empty_ReturnsNull()
    {
        _parser.Parse("").Should().BeNull();
    }

    [Fact]
    public void Parse_Whitespace_ReturnsNull()
    {
        _parser.Parse("   ").Should().BeNull();
    }

    [Fact]
    public void Parse_Move_ReturnsMoveCommand()
    {
        _parser.Parse("MOVE").Should().BeOfType<MoveCommand>();
    }

    [Fact]
    public void Parse_MoveLowercase_ReturnsMoveCommand()
    {
        _parser.Parse("move").Should().BeOfType<MoveCommand>();
    }

    [Fact]
    public void Parse_Left_ReturnsLeftCommand()
    {
        _parser.Parse("LEFT").Should().BeOfType<LeftCommand>();
    }

    [Fact]
    public void Parse_Right_ReturnsRightCommand()
    {
        _parser.Parse("RIGHT").Should().BeOfType<RightCommand>();
    }

    [Fact]
    public void Parse_Report_ReturnsReportCommand()
    {
        _parser.Parse("REPORT").Should().BeOfType<ReportCommand>();
    }

    [Fact]
    public void Parse_PlaceValid_ReturnsPlaceCommand()
    {
        _parser.Parse("PLACE 0,0,NORTH").Should().BeOfType<PlaceCommand>();
    }

    [Fact]
    public void Parse_PlaceLowercase_ReturnsPlaceCommand()
    {
        _parser.Parse("place 1,2,east").Should().BeOfType<PlaceCommand>();
    }

    [Fact]
    public void Parse_PlaceNoArgs_ReturnsNull()
    {
        _parser.Parse("PLACE").Should().BeNull();
    }

    [Fact]
    public void Parse_PlaceMissingDirection_ReturnsNull()
    {
        _parser.Parse("PLACE 0,0").Should().BeNull();
    }

    [Fact]
    public void Parse_PlaceNonInteger_ReturnsNull()
    {
        _parser.Parse("PLACE A,B,NORTH").Should().BeNull();
    }

    [Fact]
    public void Parse_PlaceInvalidDirection_ReturnsNull()
    {
        _parser.Parse("PLACE 0,0,UP").Should().BeNull();
    }

    [Fact]
    public void Parse_UnknownCommand_ReturnsNull()
    {
        _parser.Parse("JUMP").Should().BeNull();
    }
}
