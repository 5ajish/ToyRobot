# Story 1.2.1: Command Implementations

## Parent Feature: 1.2 - Command & Parsing Layer

## Status: Draft

## Dependencies

- Story 1.1.1 (Position, Direction, DirectionExtensions) — **must be complete**
- Story 1.1.2 (Robot, Table) — **must be complete**

## Story

- As a **developer**
- I want **all five command classes (Place, Move, Left, Right, Report) implemented via TDD with the ICommand interface**
- so that **each robot action is encapsulated as a testable, single-responsibility command object, with every behavior proven by tests first**

## Acceptance Criteria (ACs)

1. `ICommand` interface exists at `src/ToyRobot/Commands/ICommand.cs` with `void Execute(Robot robot, Table table)` method
2. `PlaceCommand` places robot when position is valid; ignores when off-table
3. `MoveCommand` moves robot one step in facing direction; ignores when move would leave table
4. `LeftCommand` rotates robot 90° counter-clockwise
5. `RightCommand` rotates robot 90° clockwise
6. `ReportCommand` writes `"X,Y,DIRECTION"` to stdout via `Console.WriteLine`
7. All command test cases pass (4 Place + 5 Move + 4 Left + 4 Right + 1 Report = **18 tests**)
8. **TDD discipline:** Every test written BEFORE the production code it validates

## TDD Workflow

> 🔴 **RED:** Write a failing test (code doesn't exist yet — won't compile)
> 🟢 **GREEN:** Write the minimum production code to make the test pass
> 🔵 **REFACTOR:** Clean up while keeping tests green

## Tasks / Subtasks

### Phase 1: ICommand Interface (foundation)

- [ ] Task 1.1: Create `ICommand.cs` (AC: 1)
  - [ ] Create `src/ToyRobot/Commands/ICommand.cs` per architecture §7.1
  - [ ] No tests needed — it's a contract definition only
  - [ ] **Git commit:** `feat: add ICommand interface`

### Phase 2: PlaceCommand (TDD Cycle)

- [ ] Task 2.1: 🔴 RED — Write `PlaceCommandTests.cs` (AC: 2)
  - [ ] Create `tests/ToyRobot.Tests/Commands/PlaceCommandTests.cs`
  - [ ] Write 4 test cases (see Testing section)
  - [ ] `dotnet build` → **FAILS** (PlaceCommand doesn't exist yet)
- [ ] Task 2.2: 🟢 GREEN — Create `PlaceCommand.cs` (AC: 2)
  - [ ] Create `src/ToyRobot/Commands/PlaceCommand.cs` per architecture §7.2
  - [ ] `dotnet test` → **ALL GREEN**
- [ ] Task 2.3: 🔵 REFACTOR — Review PlaceCommand
  - [ ] Verify primary constructor is used
  - [ ] **Git commit:** `feat: add PlaceCommand with boundary validation (TDD)`

### Phase 3: MoveCommand (TDD Cycle)

- [ ] Task 3.1: 🔴 RED — Write `MoveCommandTests.cs` (AC: 3)
  - [ ] Create `tests/ToyRobot.Tests/Commands/MoveCommandTests.cs`
  - [ ] Write 5 test cases — valid move + all 4 boundary edges (see Testing section)
  - [ ] `dotnet build` → **FAILS** (MoveCommand doesn't exist yet)
- [ ] Task 3.2: 🟢 GREEN — Create `MoveCommand.cs` (AC: 3)
  - [ ] Create `src/ToyRobot/Commands/MoveCommand.cs` per architecture §7.3
  - [ ] `dotnet test` → **ALL GREEN**
- [ ] Task 3.3: 🔵 REFACTOR — Review MoveCommand
  - [ ] **Git commit:** `feat: add MoveCommand with boundary protection (TDD)`

### Phase 4: LeftCommand (TDD Cycle)

- [ ] Task 4.1: 🔴 RED — Write `LeftCommandTests.cs` (AC: 4)
  - [ ] Create `tests/ToyRobot.Tests/Commands/LeftCommandTests.cs`
  - [ ] Write 4 test cases — all 4 rotation directions
  - [ ] `dotnet build` → **FAILS** (LeftCommand doesn't exist yet)
- [ ] Task 4.2: 🟢 GREEN — Create `LeftCommand.cs` (AC: 4)
  - [ ] Create `src/ToyRobot/Commands/LeftCommand.cs` per architecture §7.4
  - [ ] `dotnet test` → **ALL GREEN**
- [ ] Task 4.3: 🔵 REFACTOR — Review LeftCommand
  - [ ] **Git commit:** `feat: add LeftCommand (TDD)`

### Phase 5: RightCommand (TDD Cycle)

- [ ] Task 5.1: 🔴 RED — Write `RightCommandTests.cs` (AC: 5)
  - [ ] Create `tests/ToyRobot.Tests/Commands/RightCommandTests.cs`
  - [ ] Write 4 test cases — all 4 rotation directions
  - [ ] `dotnet build` → **FAILS** (RightCommand doesn't exist yet)
- [ ] Task 5.2: 🟢 GREEN — Create `RightCommand.cs` (AC: 5)
  - [ ] Create `src/ToyRobot/Commands/RightCommand.cs` per architecture §7.5
  - [ ] `dotnet test` → **ALL GREEN**
- [ ] Task 5.3: 🔵 REFACTOR — Review RightCommand
  - [ ] **Git commit:** `feat: add RightCommand (TDD)`

### Phase 6: ReportCommand (TDD Cycle)

- [ ] Task 6.1: 🔴 RED — Write `ReportCommandTests.cs` (AC: 6)
  - [ ] Create `tests/ToyRobot.Tests/Commands/ReportCommandTests.cs`
  - [ ] Write 1 test case — capture stdout with `StringWriter`
  - [ ] `dotnet build` → **FAILS** (ReportCommand doesn't exist yet)
- [ ] Task 6.2: 🟢 GREEN — Create `ReportCommand.cs` (AC: 6)
  - [ ] Create `src/ToyRobot/Commands/ReportCommand.cs` per architecture §7.6
  - [ ] `dotnet test` → **ALL GREEN**
- [ ] Task 6.3: 🔵 REFACTOR — Review ReportCommand
  - [ ] **Git commit:** `feat: add ReportCommand with stdout output (TDD)`

### Phase 7: Verify

- [ ] Task 7.1: Run full test suite
  - [ ] `dotnet test --verbosity normal` → 49+ tests (31 prior + 18 new), all green, zero warnings

## Dev Notes

### Architecture References

- **Exact code:** Architecture §7.1–§7.6
- **PlaceCommand truth table:** §7.2
- **MoveCommand truth table:** §7.3
- **Coding rules:** §15

### Key Implementation Details

**ICommand** — `src/ToyRobot/Commands/ICommand.cs`:

```csharp
namespace ToyRobot.Commands;

public interface ICommand
{
    void Execute(Robot robot, Table table);
}
```

> Note: `ICommand.cs` needs `using ToyRobot.Models;` and `using ToyRobot.Simulation;` for `Robot`, `Table`. Add explicit usings for now — GlobalUsings added in Story 1.3.1.

**PlaceCommand** — `src/ToyRobot/Commands/PlaceCommand.cs`:

```csharp
namespace ToyRobot.Commands;

public class PlaceCommand(int x, int y, Direction direction) : ICommand
{
    public void Execute(Robot robot, Table table)
    {
        if (table.IsValidPosition(new Position(x, y)))
            robot.Place(x, y, direction);
    }
}
```

**MoveCommand** — `src/ToyRobot/Commands/MoveCommand.cs`:

```csharp
namespace ToyRobot.Commands;

public class MoveCommand : ICommand
{
    public void Execute(Robot robot, Table table)
    {
        var newPosition = robot.NextPosition();
        if (table.IsValidPosition(newPosition))
            robot.MoveTo(newPosition);
    }
}
```

**LeftCommand** — `src/ToyRobot/Commands/LeftCommand.cs`:

```csharp
namespace ToyRobot.Commands;

public class LeftCommand : ICommand
{
    public void Execute(Robot robot, Table table) => robot.TurnLeft();
}
```

**RightCommand** — `src/ToyRobot/Commands/RightCommand.cs`:

```csharp
namespace ToyRobot.Commands;

public class RightCommand : ICommand
{
    public void Execute(Robot robot, Table table) => robot.TurnRight();
}
```

**ReportCommand** — `src/ToyRobot/Commands/ReportCommand.cs`:

```csharp
namespace ToyRobot.Commands;

public class ReportCommand : ICommand
{
    public void Execute(Robot robot, Table table)
    {
        Console.WriteLine(robot.Report());
    }
}
```

### Source Tree (this story adds)

```
src/ToyRobot/
├── Commands/
│   ├── ICommand.cs              ← NEW
│   ├── PlaceCommand.cs          ← NEW
│   ├── MoveCommand.cs           ← NEW
│   ├── LeftCommand.cs           ← NEW
│   ├── RightCommand.cs          ← NEW
│   └── ReportCommand.cs         ← NEW
tests/ToyRobot.Tests/
├── Commands/
│   ├── PlaceCommandTests.cs     ← NEW
│   ├── MoveCommandTests.cs      ← NEW
│   ├── LeftCommandTests.cs      ← NEW
│   ├── RightCommandTests.cs     ← NEW
│   └── ReportCommandTests.cs    ← NEW
```

### Coding Rules

- R4: Primary constructor for `PlaceCommand`
- R6: One type per file
- R7: Only `ReportCommand` touches `Console.WriteLine` — no console in domain logic
- R10: Test naming `MethodName_Scenario_ExpectedResult`
- R11: FluentAssertions `.Should()` syntax

### Testing

Dev Note: Story Requires the following tests:

- [ ] xUnit Unit Tests: (nextToFile: false), coverage requirement: 100%
  - `tests/ToyRobot.Tests/Commands/PlaceCommandTests.cs` — write FIRST in Task 2.1
  - `tests/ToyRobot.Tests/Commands/MoveCommandTests.cs` — write FIRST in Task 3.1
  - `tests/ToyRobot.Tests/Commands/LeftCommandTests.cs` — write FIRST in Task 4.1
  - `tests/ToyRobot.Tests/Commands/RightCommandTests.cs` — write FIRST in Task 5.1
  - `tests/ToyRobot.Tests/Commands/ReportCommandTests.cs` — write FIRST in Task 6.1

**PlaceCommandTests (write in Task 2.1 — before PlaceCommand.cs exists):**

| Test Name | Setup | Expected |
|---|---|---|
| `Execute_ValidPosition_PlacesRobot` | `PlaceCommand(0,0,North)` on 5×5 table | Robot placed at (0,0) facing North |
| `Execute_OffTable_DoesNotPlace` | `PlaceCommand(5,5,North)` on 5×5 table | `robot.IsPlaced == false` |
| `Execute_NegativeCoords_DoesNotPlace` | `PlaceCommand(-1,0,North)` on 5×5 table | `robot.IsPlaced == false` |
| `Execute_EdgePosition_PlacesRobot` | `PlaceCommand(4,4,South)` on 5×5 table | Robot placed at (4,4) facing South |

**MoveCommandTests (write in Task 3.1 — before MoveCommand.cs exists):**

| Test Name | Robot State | Expected |
|---|---|---|
| `Execute_ValidMove_RobotMoves` | (0,0,North) | Position == (0,1) |
| `Execute_WouldFallNorth_RobotStays` | (0,4,North) | Position == (0,4) |
| `Execute_WouldFallEast_RobotStays` | (4,0,East) | Position == (4,0) |
| `Execute_WouldFallSouth_RobotStays` | (0,0,South) | Position == (0,0) |
| `Execute_WouldFallWest_RobotStays` | (0,0,West) | Position == (0,0) |

**LeftCommandTests (write in Task 4.1 — before LeftCommand.cs exists):**

| Test Name | Initial Direction | Expected Direction |
|---|---|---|
| `Execute_FacingNorth_TurnsWest` | North | West |
| `Execute_FacingWest_TurnsSouth` | West | South |
| `Execute_FacingSouth_TurnsEast` | South | East |
| `Execute_FacingEast_TurnsNorth` | East | North |

**RightCommandTests (write in Task 5.1 — before RightCommand.cs exists):**

| Test Name | Initial Direction | Expected Direction |
|---|---|---|
| `Execute_FacingNorth_TurnsEast` | North | East |
| `Execute_FacingEast_TurnsSouth` | East | South |
| `Execute_FacingSouth_TurnsWest` | South | West |
| `Execute_FacingWest_TurnsNorth` | West | North |

**ReportCommandTests (write in Task 6.1 — before ReportCommand.cs exists):**

| Test Name | Robot State | Expected stdout |
|---|---|---|
| `Execute_PlacedRobot_OutputsPosition` | (1,2,East) | `"1,2,EAST"` followed by newline |

> **Tip for ReportCommand test:** Use `StringWriter` to capture stdout:
> ```csharp
> var sw = new StringWriter();
> Console.SetOut(sw);
> // ... execute ...
> sw.ToString().Trim().Should().Be("1,2,EAST");
> ```

### Git Commit Cadence

```
1. feat: add ICommand interface
2. feat: add PlaceCommand with boundary validation (TDD)
3. feat: add MoveCommand with boundary protection (TDD)
4. feat: add LeftCommand (TDD)
5. feat: add RightCommand (TDD)
6. feat: add ReportCommand with stdout output (TDD)
```

Manual Test Steps:

- Run `dotnet test --verbosity normal` — 49+ tests, all green, zero warnings

## Definition of Done

- [ ] `ICommand` interface created
- [ ] PlaceCommand tests written FIRST, then PlaceCommand implemented
- [ ] MoveCommand tests written FIRST, then MoveCommand implemented
- [ ] LeftCommand tests written FIRST, then LeftCommand implemented
- [ ] RightCommand tests written FIRST, then RightCommand implemented
- [ ] ReportCommand tests written FIRST, then ReportCommand implemented
- [ ] All 18 command test cases pass
- [ ] `dotnet test` all green, zero warnings
- [ ] Git history shows test-first commits
