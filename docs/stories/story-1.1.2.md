# Story 1.1.2: Robot Entity & Table Boundary

## Parent Feature: 1.1 - Domain Models

## Status: Draft

## Story

- As a **developer**
- I want **the Robot entity and Table boundary model implemented via TDD**
- so that **the domain can represent robot state (position, direction, placement) and validate positions against table bounds, with every behavior proven by tests first**

## Acceptance Criteria (ACs)

1. `Robot` class exists at `src/ToyRobot/Models/Robot.cs` with properties: `Position?`, `Direction?`, `IsPlaced`
2. `Robot.Place(x, y, direction)` sets position and direction, `IsPlaced` becomes `true`
3. `Robot.NextPosition()` returns calculated next position without mutating state; throws `InvalidOperationException` if not placed
4. `Robot.MoveTo(position)` updates position
5. `Robot.TurnLeft()` and `TurnRight()` rotate direction; no-op if not placed
6. `Robot.Report()` returns `"X,Y,DIRECTION"` format (e.g. `"1,2,EAST"`)
7. `Robot.Place()` called twice overwrites previous state
8. `Table` class exists at `src/ToyRobot/Simulation/Table.cs` with configurable width/height (default 5×5)
9. `Table.IsValidPosition()` returns `true` for positions in [0, width-1] × [0, height-1], `false` otherwise
10. All Robot + Table tests pass
11. **TDD discipline:** Every test written BEFORE the production code it validates

## TDD Workflow

> 🔴 **RED:** Write a failing test (code doesn't exist yet — won't compile)
> 🟢 **GREEN:** Write the minimum production code to make the test pass
> 🔵 **REFACTOR:** Clean up while keeping tests green

## Tasks / Subtasks

### Phase 1: Robot Entity (TDD Cycle)

- [ ] Task 1.1: 🔴 RED — Write `RobotTests.cs`  (AC: 1-7)
  - [ ] Create `tests/ToyRobot.Tests/Models/RobotTests.cs`
  - [ ] Write all 9 test cases (see Testing section)
  - [ ] `dotnet build` → **FAILS** (Robot class doesn't exist yet)
- [ ] Task 1.2: 🟢 GREEN — Create `Robot.cs` (AC: 1-7)
  - [ ] Create `src/ToyRobot/Models/Robot.cs` per architecture §6.4
  - [ ] `dotnet test` → **ALL GREEN** (previous 15 tests + new 9 Robot tests)
- [ ] Task 1.3: 🔵 REFACTOR — Review Robot class
  - [ ] Verify no unnecessary methods or properties
  - [ ] Ensure nullable types are used correctly (`Position?`, `Direction?`)
  - [ ] **Git commit:** `feat: add Robot entity with Place, Move, Turn, Report (TDD)`

### Phase 2: Table Boundary (TDD Cycle)

- [ ] Task 2.1: 🔴 RED — Write Table boundary tests (AC: 8-9)
  - [ ] Add Table tests to a new file `tests/ToyRobot.Tests/Simulation/TableTests.cs`
  - [ ] Write 7 boundary test cases (see Testing section)
  - [ ] `dotnet build` → **FAILS** (Table class doesn't exist yet)
- [ ] Task 2.2: 🟢 GREEN — Create `Table.cs` (AC: 8-9)
  - [ ] Create `src/ToyRobot/Simulation/Table.cs` per architecture §6.5
  - [ ] `dotnet test` → **ALL GREEN**
- [ ] Task 2.3: 🔵 REFACTOR — Review Table class
  - [ ] Verify primary constructor is used
  - [ ] **Git commit:** `feat: add Table boundary with configurable dimensions (TDD)`

### Phase 3: Verify

- [ ] Task 3.1: Run full test suite
  - [ ] `dotnet test --verbosity normal` → 31+ tests, all green, zero warnings

## Dev Notes

### Architecture References

- **Exact code:** Architecture §6.4 (Robot), §6.5 (Table)
- **Method contracts:** Architecture §6.4 method contract table
- **Boundary truth table:** Architecture §6.5

### Key Implementation Details

**Robot** — `src/ToyRobot/Models/Robot.cs`:

```csharp
namespace ToyRobot.Models;

public class Robot
{
    public Position? Position { get; private set; }
    public Direction? Direction { get; private set; }
    public bool IsPlaced => Position is not null && Direction is not null;

    public void Place(int x, int y, Direction direction)
    {
        Position = new Position(x, y);
        Direction = direction;
    }

    public Position NextPosition()
    {
        if (!IsPlaced) throw new InvalidOperationException("Robot is not placed.");
        var vector = Direction!.Value.ToMovementVector();
        return new Position(Position!.X + vector.X, Position.Y + vector.Y);
    }

    public void MoveTo(Position position) => Position = position;

    public void TurnLeft()
    {
        if (IsPlaced) Direction = Direction!.Value.TurnLeft();
    }

    public void TurnRight()
    {
        if (IsPlaced) Direction = Direction!.Value.TurnRight();
    }

    public string Report() => $"{Position!.X},{Position!.Y},{Direction!.Value.ToString().ToUpperInvariant()}";
}
```

**Table** — `src/ToyRobot/Simulation/Table.cs`:

```csharp
namespace ToyRobot.Simulation;

public class Table(int width = 5, int height = 5)
{
    public bool IsValidPosition(Position pos) =>
        pos.X >= 0 && pos.X < width && pos.Y >= 0 && pos.Y < height;
}
```

> Note: `Table.cs` needs `using ToyRobot.Models;` for `Position`. Add explicit using for now — GlobalUsings added in Story 1.3.1.

### Source Tree (this story adds)

```
src/ToyRobot/
├── Models/
│   └── Robot.cs                ← NEW
├── Simulation/
│   └── Table.cs                ← NEW
tests/ToyRobot.Tests/
├── Models/
│   └── RobotTests.cs           ← NEW
├── Simulation/
│   └── TableTests.cs           ← NEW
```

### Coding Rules

- R4: Primary constructor for `Table`
- R5: Pattern matching — `is not null`, `is { }`
- R6: One type per file
- R9: Nullable reference types — `Position?`, `Direction?`

### Testing

Dev Note: Story Requires the following tests:

- [ ] xUnit Unit Tests: (nextToFile: false), coverage requirement: 100%
  - `tests/ToyRobot.Tests/Models/RobotTests.cs` — write FIRST in Task 1.1
  - `tests/ToyRobot.Tests/Simulation/TableTests.cs` — write FIRST in Task 2.1

**RobotTests (write in Task 1.1 — before Robot.cs exists):**

| Test Name | Setup | Action | Expected |
|---|---|---|---|
| `IsPlaced_Initially_ReturnsFalse` | `new Robot()` | — | `robot.IsPlaced.Should().BeFalse()` |
| `Place_ValidArgs_SetsPositionAndDirection` | `new Robot()` | `Place(1, 2, East)` | `Position == (1,2)`, `Direction == East`, `IsPlaced == true` |
| `NextPosition_FacingNorth_ReturnsOneStepNorth` | Placed at `(0,0,North)` | `NextPosition()` | `new Position(0, 1)` |
| `NextPosition_FacingEast_ReturnsOneStepEast` | Placed at `(2,3,East)` | `NextPosition()` | `new Position(3, 3)` |
| `MoveTo_NewPosition_UpdatesPosition` | Placed at `(0,0,North)` | `MoveTo(new Position(0,1))` | `Position == (0,1)` |
| `TurnLeft_FacingNorth_DirectionBecomesWest` | Placed at `(0,0,North)` | `TurnLeft()` | `Direction == West` |
| `TurnRight_FacingNorth_DirectionBecomesEast` | Placed at `(0,0,North)` | `TurnRight()` | `Direction == East` |
| `Report_Placed_ReturnsFormattedString` | Placed at `(1,2,East)` | `Report()` | `"1,2,EAST"` |
| `Place_CalledTwice_OverwritesState` | Placed at `(0,0,North)` | `Place(3,3,South)` | `Position == (3,3)`, `Direction == South` |

**TableTests (write in Task 2.1 — before Table.cs exists):**

| Test Name | Position | Expected |
|---|---|---|
| `IsValidPosition_Origin_ReturnsTrue` | `(0,0)` | `true` |
| `IsValidPosition_MaxCorner_ReturnsTrue` | `(4,4)` | `true` |
| `IsValidPosition_Center_ReturnsTrue` | `(2,2)` | `true` |
| `IsValidPosition_XOutOfRange_ReturnsFalse` | `(5,0)` | `false` |
| `IsValidPosition_YOutOfRange_ReturnsFalse` | `(0,5)` | `false` |
| `IsValidPosition_NegativeX_ReturnsFalse` | `(-1,0)` | `false` |
| `IsValidPosition_NegativeY_ReturnsFalse` | `(0,-1)` | `false` |

### Git Commit Cadence

```
1. feat: add Robot entity with Place, Move, Turn, Report (TDD)
2. feat: add Table boundary with configurable dimensions (TDD)
```

Manual Test Steps:

- Run `dotnet test --verbosity normal` — 31+ tests, all green, zero warnings

## Definition of Done

- [ ] Robot tests written FIRST, then Robot class implemented
- [ ] Table tests written FIRST, then Table class implemented
- [ ] All 16 test cases pass (9 Robot + 7 Table)
- [ ] `dotnet test` all green, zero warnings
- [ ] Git history shows test-first commits
