# Story 1.1.1: Project Setup & Core Value Types

## Parent Feature: 1.1 - Domain Models

## Status: Draft

## Story

- As a **developer**
- I want **the .NET solution scaffolded and core value types (Position, Direction, DirectionExtensions) implemented via TDD**
- so that **the foundation layer is established for all subsequent domain logic, with tests proving every behavior before code is written**

## Acceptance Criteria (ACs)

1. .NET 8.0 solution is scaffolded with `src/ToyRobot` console project and `tests/ToyRobot.Tests` xUnit project
2. `ToyRobot.Tests` references `ToyRobot` project and has `FluentAssertions` package installed
3. `Position` record exists at `src/ToyRobot/Models/Position.cs` — immutable, value equality, `ToString()` returns `"X,Y"`
4. `Direction` enum exists at `src/ToyRobot/Models/Direction.cs` — has members `North`, `East`, `South`, `West`
5. `DirectionExtensions` exists at `src/ToyRobot/Models/DirectionExtensions.cs` — provides `TurnLeft()`, `TurnRight()`, `ToMovementVector()` extension methods
6. All 12 `DirectionExtensions` test cases pass (4 TurnLeft + 4 TurnRight + 4 ToMovementVector)
7. `Position` equality and `ToString` tests pass
8. `dotnet build` succeeds with zero warnings. `dotnet test` shows all tests green.
9. **TDD discipline:** Every test was written BEFORE the production code it validates. Git history shows test-first commits.

## TDD Workflow

> 🔴 **RED:** Write a failing test (code doesn't exist yet — won't compile)
> 🟢 **GREEN:** Write the minimum production code to make the test pass
> 🔵 **REFACTOR:** Clean up while keeping tests green

## Tasks / Subtasks

### Phase 0: Scaffold (no tests needed)

- [ ] Task 0.1: Scaffold .NET solution (AC: 1, 2)
  - [ ] Run scaffolding commands from architecture §5 in `C:\Dev\ToyRobot`
  - [ ] Verify `dotnet build` succeeds
  - [ ] Delete auto-generated `Program.cs` content (replace with empty `// Entry point - implemented in Story 1.3.1`)
  - [ ] Delete auto-generated `UnitTest1.cs` in test project
  - [ ] **Git commit:** `chore: scaffold .NET solution with ToyRobot + ToyRobot.Tests projects`

### Phase 1: Position (TDD Cycle)

- [ ] Task 1.1: 🔴 RED — Write `PositionTests.cs` (AC: 3, 7)
  - [ ] Create `tests/ToyRobot.Tests/Models/PositionTests.cs`
  - [ ] Write 3 test cases (see Testing section below)
  - [ ] `dotnet build` → **FAILS** (Position doesn't exist yet — this is expected)
- [ ] Task 1.2: 🟢 GREEN — Create `Position.cs` (AC: 3)
  - [ ] Create `src/ToyRobot/Models/Position.cs` per architecture §6.1
  - [ ] `dotnet test` → **ALL GREEN**
- [ ] Task 1.3: 🔵 REFACTOR — Review Position
  - [ ] Verify record is clean and minimal. Nothing to refactor for a one-liner.
  - [ ] **Git commit:** `feat: add Position record with value equality (TDD: red→green→refactor)`

### Phase 2: Direction + DirectionExtensions (TDD Cycle)

- [ ] Task 2.1: 🔴 RED — Write `DirectionExtensionsTests.cs` (AC: 5, 6)
  - [ ] Create `tests/ToyRobot.Tests/Models/DirectionExtensionsTests.cs`
  - [ ] Write all 12 test cases (4 TurnLeft + 4 TurnRight + 4 ToMovementVector)
  - [ ] `dotnet build` → **FAILS** (Direction enum and extensions don't exist yet)
- [ ] Task 2.2: 🟢 GREEN — Create `Direction.cs` + `DirectionExtensions.cs` (AC: 4, 5)
  - [ ] Create `src/ToyRobot/Models/Direction.cs` per architecture §6.2
  - [ ] Create `src/ToyRobot/Models/DirectionExtensions.cs` per architecture §6.3
  - [ ] `dotnet test` → **ALL GREEN** (all 15 tests: 3 Position + 12 Direction)
- [ ] Task 2.3: 🔵 REFACTOR — Review Direction logic
  - [ ] Verify switch expressions are clean and exhaustive
  - [ ] **Git commit:** `feat: add Direction enum with TurnLeft, TurnRight, ToMovementVector extensions (TDD)`

### Phase 3: Verify

- [ ] Task 3.1: Run full test suite
  - [ ] `dotnet test --verbosity normal` → 15 tests, all green, zero warnings

## Dev Notes

### Architecture References

- **Architecture doc:** `docs/architecture.md`
- **Exact code:** §6.1 (Position), §6.2 (Direction), §6.3 (DirectionExtensions)
- **Scaffolding commands:** §5
- **Project structure:** §4

### Key Implementation Details

**Scaffolding commands (run from `C:\Dev\ToyRobot`):**

```bash
dotnet new sln -n ToyRobot
dotnet new console -n ToyRobot -o src/ToyRobot --framework net8.0
dotnet new xunit -n ToyRobot.Tests -o tests/ToyRobot.Tests --framework net8.0
dotnet sln add src/ToyRobot/ToyRobot.csproj
dotnet sln add tests/ToyRobot.Tests/ToyRobot.Tests.csproj
dotnet add tests/ToyRobot.Tests reference src/ToyRobot
dotnet add tests/ToyRobot.Tests package FluentAssertions
```

**Position** — `src/ToyRobot/Models/Position.cs`:

```csharp
namespace ToyRobot.Models;

public record Position(int X, int Y)
{
    public override string ToString() => $"{X},{Y}";
}
```

**Direction** — `src/ToyRobot/Models/Direction.cs`:

```csharp
namespace ToyRobot.Models;

public enum Direction
{
    North,
    East,
    South,
    West
}
```

**DirectionExtensions** — `src/ToyRobot/Models/DirectionExtensions.cs`:

```csharp
namespace ToyRobot.Models;

public static class DirectionExtensions
{
    public static Direction TurnLeft(this Direction direction) => direction switch
    {
        Direction.North => Direction.West,
        Direction.West  => Direction.South,
        Direction.South => Direction.East,
        Direction.East  => Direction.North,
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };

    public static Direction TurnRight(this Direction direction) => direction switch
    {
        Direction.North => Direction.East,
        Direction.East  => Direction.South,
        Direction.South => Direction.West,
        Direction.West  => Direction.North,
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };

    public static Position ToMovementVector(this Direction direction) => direction switch
    {
        Direction.North => new Position(0, 1),
        Direction.East  => new Position(1, 0),
        Direction.South => new Position(0, -1),
        Direction.West  => new Position(-1, 0),
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };
}
```

### Source Tree (this story)

```
src/ToyRobot/
├── Models/
│   ├── Position.cs
│   ├── Direction.cs
│   └── DirectionExtensions.cs
tests/ToyRobot.Tests/
├── Models/
│   ├── PositionTests.cs
│   └── DirectionExtensionsTests.cs
```

### Coding Rules

- R1: File-scoped namespaces (`namespace X;`)
- R2: `Position` MUST be a `record`, never `class`
- R3: Switch expressions for all mapping logic
- R6: One type per file
- R10: Test naming `MethodName_Scenario_ExpectedResult`
- R11: FluentAssertions `.Should()` syntax

### Testing

Dev Note: Story Requires the following tests:

- [ ] xUnit Unit Tests: (nextToFile: false), coverage requirement: 100%
  - `tests/ToyRobot.Tests/Models/PositionTests.cs` — write FIRST in Task 1.1
  - `tests/ToyRobot.Tests/Models/DirectionExtensionsTests.cs` — write FIRST in Task 2.1

**PositionTests (write in Task 1.1 — before Position.cs exists):**

| Test Name | Action | Expected |
|---|---|---|
| `Equality_SameValues_AreEqual` | `new Position(1,2) == new Position(1,2)` | `true` |
| `Equality_DifferentValues_AreNotEqual` | `new Position(1,2) == new Position(3,4)` | `false` |
| `ToString_ReturnsCommaSeparated` | `new Position(3,4).ToString()` | `"3,4"` |

**DirectionExtensionsTests (write in Task 2.1 — before Direction.cs exists):**

| Test Name | Input | Expected |
|---|---|---|
| `TurnLeft_FromNorth_ReturnsWest` | `Direction.North.TurnLeft()` | `Direction.West` |
| `TurnLeft_FromWest_ReturnsSouth` | `Direction.West.TurnLeft()` | `Direction.South` |
| `TurnLeft_FromSouth_ReturnsEast` | `Direction.South.TurnLeft()` | `Direction.East` |
| `TurnLeft_FromEast_ReturnsNorth` | `Direction.East.TurnLeft()` | `Direction.North` |
| `TurnRight_FromNorth_ReturnsEast` | `Direction.North.TurnRight()` | `Direction.East` |
| `TurnRight_FromEast_ReturnsSouth` | `Direction.East.TurnRight()` | `Direction.South` |
| `TurnRight_FromSouth_ReturnsWest` | `Direction.South.TurnRight()` | `Direction.West` |
| `TurnRight_FromWest_ReturnsNorth` | `Direction.West.TurnRight()` | `Direction.North` |
| `ToMovementVector_North_Returns0_1` | `Direction.North.ToMovementVector()` | `new Position(0, 1)` |
| `ToMovementVector_East_Returns1_0` | `Direction.East.ToMovementVector()` | `new Position(1, 0)` |
| `ToMovementVector_South_Returns0_Neg1` | `Direction.South.ToMovementVector()` | `new Position(0, -1)` |
| `ToMovementVector_West_ReturnsNeg1_0` | `Direction.West.ToMovementVector()` | `new Position(-1, 0)` |

### Git Commit Cadence

```
1. chore: scaffold .NET solution with ToyRobot + ToyRobot.Tests projects
2. feat: add Position record with value equality (TDD: red→green→refactor)
3. feat: add Direction enum with TurnLeft, TurnRight, ToMovementVector extensions (TDD)
```

Manual Test Steps:

- Run `dotnet test --verbosity normal` — 15 tests, all green, zero warnings

## Definition of Done

- [ ] Solution scaffolded and builds
- [ ] Position tests written FIRST, then Position record implemented
- [ ] DirectionExtensions tests written FIRST, then Direction + Extensions implemented
- [ ] All 15 test cases pass
- [ ] `dotnet build` zero warnings, `dotnet test` all green
- [ ] Git history shows test-first commits
