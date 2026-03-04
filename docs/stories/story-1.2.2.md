# Story 1.2.2: Command Parser

## Parent Feature: 1.2 - Command & Parsing Layer

## Status: Draft

## Dependencies

- Story 1.2.1 (Command Implementations) — **must be complete**

## Story

- As a **developer**
- I want **the CommandParser implemented via TDD to convert raw string input into typed ICommand objects**
- so that **user input is cleanly translated into domain commands with full case-insensitivity, whitespace tolerance, and robust error handling — all proven by tests first**

## Acceptance Criteria (ACs)

1. `CommandParser` class exists at `src/ToyRobot/Parsing/CommandParser.cs`
2. `Parse(string? input)` returns `ICommand?` — returns `null` for invalid/unrecognized input
3. Parses `MOVE`, `LEFT`, `RIGHT`, `REPORT` into respective command objects
4. Parses `PLACE X,Y,F` into `PlaceCommand` with integer coordinates and valid direction
5. Input is case-insensitive (`"move"` == `"MOVE"`)
6. Handles whitespace: `null`, empty `""`, whitespace-only `"   "` all return `null`
7. Handles malformed PLACE: missing args, non-integer coords, invalid direction, wrong arg count — all return `null`
8. Unknown commands (e.g., `"JUMP"`) return `null`
9. All 15 parser test cases pass
10. **TDD discipline:** Every test written BEFORE the production code it validates

## TDD Workflow

> 🔴 **RED:** Write a failing test (code doesn't exist yet — won't compile)
> 🟢 **GREEN:** Write the minimum production code to make the test pass
> 🔵 **REFACTOR:** Clean up while keeping tests green

## Tasks / Subtasks

### Phase 1: CommandParser (TDD Cycle)

- [ ] Task 1.1: 🔴 RED — Write `CommandParserTests.cs` (AC: 2-9)
  - [ ] Create `tests/ToyRobot.Tests/Parsing/CommandParserTests.cs`
  - [ ] Write all 15 test cases (see Testing section)
  - [ ] `dotnet build` → **FAILS** (CommandParser doesn't exist yet)
- [ ] Task 1.2: 🟢 GREEN — Create `CommandParser.cs` (AC: 1-8)
  - [ ] Create `src/ToyRobot/Parsing/CommandParser.cs` per architecture §8
  - [ ] `dotnet test` → **ALL GREEN**
- [ ] Task 1.3: 🔵 REFACTOR — Review CommandParser
  - [ ] Verify switch expression is clean and exhaustive
  - [ ] Verify `ParsePlace` private method handles all edge cases
  - [ ] Consider: is the code as readable as it can be?
  - [ ] **Git commit:** `feat: add CommandParser with full input validation (TDD)`

### Phase 2: Verify

- [ ] Task 2.1: Run full test suite
  - [ ] `dotnet test --verbosity normal` → 64+ tests (49 prior + 15 new), all green, zero warnings

## Dev Notes

### Architecture References

- **Exact code:** Architecture §8
- **Parser truth table:** Architecture §8 (14 rows + 1 whitespace-handling row)
- **Coding rules:** §15

### Key Implementation Details

**CommandParser** — `src/ToyRobot/Parsing/CommandParser.cs`:

```csharp
namespace ToyRobot.Parsing;

public class CommandParser
{
    public ICommand? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var parts = input.Trim().ToUpperInvariant().Split(' ', 2);

        return parts[0] switch
        {
            "MOVE"   => new MoveCommand(),
            "LEFT"   => new LeftCommand(),
            "RIGHT"  => new RightCommand(),
            "REPORT" => new ReportCommand(),
            "PLACE"  => ParsePlace(parts.Length > 1 ? parts[1] : null),
            _        => null
        };
    }

    private static PlaceCommand? ParsePlace(string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;

        var parts = args.Split(',');
        if (parts.Length != 3)
            return null;

        if (!int.TryParse(parts[0].Trim(), out var x) ||
            !int.TryParse(parts[1].Trim(), out var y) ||
            !Enum.TryParse<Direction>(parts[2].Trim(), ignoreCase: true, out var direction))
            return null;

        return new PlaceCommand(x, y, direction);
    }
}
```

> Note: `CommandParser.cs` needs usings for `ToyRobot.Commands` and `ToyRobot.Models`. Add explicit usings for now — GlobalUsings added in Story 1.3.1.

### Source Tree (this story adds)

```
src/ToyRobot/
├── Parsing/
│   └── CommandParser.cs          ← NEW
tests/ToyRobot.Tests/
├── Parsing/
│   └── CommandParserTests.cs     ← NEW
```

### Coding Rules

- R3: Switch expressions for command dispatch
- R5: Pattern matching — `is { }` for null-check-with-assignment
- R6: One type per file
- R9: Nullable reference types — `string?` input, `ICommand?` return
- R10: Test naming `MethodName_Scenario_ExpectedResult`
- R11: FluentAssertions `.Should()` syntax

### Testing

Dev Note: Story Requires the following tests:

- [ ] xUnit Unit Tests: (nextToFile: false), coverage requirement: 100%
  - `tests/ToyRobot.Tests/Parsing/CommandParserTests.cs` — write FIRST in Task 1.1

**CommandParserTests (write in Task 1.1 — before CommandParser.cs exists):**

| Test Name | Input | Expected |
|---|---|---|
| `Parse_Null_ReturnsNull` | `null` | `null` |
| `Parse_Empty_ReturnsNull` | `""` | `null` |
| `Parse_Whitespace_ReturnsNull` | `"   "` | `null` |
| `Parse_Move_ReturnsMoveCommand` | `"MOVE"` | `MoveCommand` |
| `Parse_MoveLowercase_ReturnsMoveCommand` | `"move"` | `MoveCommand` |
| `Parse_Left_ReturnsLeftCommand` | `"LEFT"` | `LeftCommand` |
| `Parse_Right_ReturnsRightCommand` | `"RIGHT"` | `RightCommand` |
| `Parse_Report_ReturnsReportCommand` | `"REPORT"` | `ReportCommand` |
| `Parse_PlaceValid_ReturnsPlaceCommand` | `"PLACE 0,0,NORTH"` | `PlaceCommand` |
| `Parse_PlaceLowercase_ReturnsPlaceCommand` | `"place 1,2,east"` | `PlaceCommand` |
| `Parse_PlaceNoArgs_ReturnsNull` | `"PLACE"` | `null` |
| `Parse_PlaceMissingDirection_ReturnsNull` | `"PLACE 0,0"` | `null` |
| `Parse_PlaceNonInteger_ReturnsNull` | `"PLACE A,B,NORTH"` | `null` |
| `Parse_PlaceInvalidDirection_ReturnsNull` | `"PLACE 0,0,UP"` | `null` |
| `Parse_UnknownCommand_ReturnsNull` | `"JUMP"` | `null` |

> **Test assertion pattern:**
> - For null results: `result.Should().BeNull();`
> - For typed commands: `result.Should().BeOfType<MoveCommand>();`
> - For PlaceCommand, test both type and that a valid PlaceCommand is returned

### Git Commit Cadence

```
1. feat: add CommandParser with full input validation (TDD)
```

Manual Test Steps:

- Run `dotnet test --verbosity normal` — 64+ tests, all green, zero warnings

## Definition of Done

- [ ] CommandParser tests written FIRST (all 15 cases), then CommandParser implemented
- [ ] All 15 parser test cases pass
- [ ] `dotnet test` all green, zero warnings
- [ ] Git history shows test-first commit
