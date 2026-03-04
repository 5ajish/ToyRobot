# Story 1.3.1: Simulator & Console Application

## Parent Feature: 1.3 - Simulation & Application Layer

## Status: Draft

## Dependencies

- Story 1.2.2 (CommandParser) — **must be complete**
- All prior stories (1.1.1, 1.1.2, 1.2.1, 1.2.2) — **must be complete**

## Story

- As a **developer**
- I want **the Simulator orchestrator implemented via TDD, plus the console application entry point and test data files wired up**
- so that **the complete Toy Robot Simulator is functional end-to-end — accepting commands from stdin and producing correct output on stdout**

## Acceptance Criteria (ACs)

1. `Simulator` class exists at `src/ToyRobot/Simulation/Simulator.cs` with `Execute(ICommand command)` method
2. `Simulator` ignores all non-PLACE commands before robot is placed (Constraint C3)
3. `Simulator` delegates to `command.Execute(robot, table)` after robot is placed
4. `Simulator` integration tests pass all 3 spec examples + 4 edge-case scenarios
5. `GlobalUsings.cs` exists with all 4 global usings
6. `Program.cs` implements the REPL loop with interactive/piped mode detection
7. Test data files exist: `example1.txt`, `example2.txt`, `example3.txt`, `boundary-test.txt`, `invalid-commands.txt`
8. `dotnet run < test-data/example1.txt` produces `0,1,NORTH` (and so on for all test files)
9. All automated tests pass. Full suite: 71+ tests, zero warnings.
10. **TDD discipline:** Simulator tests written BEFORE Simulator code. Program.cs validated via manual test files (pragmatic approach).

## TDD Workflow

> 🔴 **RED:** Write a failing test (code doesn't exist yet — won't compile)
> 🟢 **GREEN:** Write the minimum production code to make the test pass
> 🔵 **REFACTOR:** Clean up while keeping tests green
>
> **Exception:** `Program.cs` is thin glue code — validated by manual test data files, not unit tests.

## Tasks / Subtasks

### Phase 1: Simulator (TDD Cycle)

- [ ] Task 1.1: 🔴 RED — Write `SimulatorTests.cs` (AC: 1-4)
  - [ ] Create `tests/ToyRobot.Tests/Simulation/SimulatorTests.cs`
  - [ ] Write all 7 integration test cases (see Testing section)
  - [ ] Use `StringWriter` to capture stdout for REPORT assertions
  - [ ] `dotnet build` → **FAILS** (Simulator class doesn't exist yet)
- [ ] Task 1.2: 🟢 GREEN — Create `Simulator.cs` (AC: 1-3)
  - [ ] Create `src/ToyRobot/Simulation/Simulator.cs` per architecture §9
  - [ ] `dotnet test` → **ALL GREEN**
- [ ] Task 1.3: 🔵 REFACTOR — Review Simulator
  - [ ] Verify guard clause is clean: `if (!_robot.IsPlaced && command is not PlaceCommand) return;`
  - [ ] **Git commit:** `feat: add Simulator with placement guard (TDD)`

### Phase 2: GlobalUsings + Program.cs (wiring — not TDD)

- [ ] Task 2.1: Create `GlobalUsings.cs` (AC: 5)
  - [ ] Create `src/ToyRobot/GlobalUsings.cs` per architecture §11
  - [ ] Remove any explicit `using` statements in source files that are now covered by global usings
  - [ ] `dotnet build` → succeeds
- [ ] Task 2.2: Create `Program.cs` (AC: 6)
  - [ ] Replace placeholder content in `src/ToyRobot/Program.cs` per architecture §10
  - [ ] `dotnet build` → succeeds
  - [ ] **Git commit:** `feat: add GlobalUsings and Program.cs REPL loop`

### Phase 3: Test Data Files (manual validation)

- [ ] Task 3.1: Create test data files (AC: 7)
  - [ ] Create `test-data/example1.txt` per architecture §13
  - [ ] Create `test-data/example2.txt` per architecture §13
  - [ ] Create `test-data/example3.txt` per architecture §13
  - [ ] Create `test-data/boundary-test.txt` per architecture §13
  - [ ] Create `test-data/invalid-commands.txt` per architecture §13
  - [ ] **Git commit:** `test: add test data files for manual E2E validation`

### Phase 4: Manual End-to-End Validation (AC: 8)

- [ ] Task 4.1: Validate with test data files
  - [ ] `dotnet run --project src/ToyRobot < test-data/example1.txt` → output: `0,1,NORTH`
  - [ ] `dotnet run --project src/ToyRobot < test-data/example2.txt` → output: `0,0,WEST`
  - [ ] `dotnet run --project src/ToyRobot < test-data/example3.txt` → output: `3,3,NORTH`
  - [ ] `dotnet run --project src/ToyRobot < test-data/boundary-test.txt` → output (4 lines):
    ```
    0,0,SOUTH
    4,4,NORTH
    0,0,WEST
    4,0,EAST
    ```
  - [ ] `dotnet run --project src/ToyRobot < test-data/invalid-commands.txt` → output (2 lines):
    ```
    0,0,NORTH
    0,1,NORTH
    ```
  - [ ] Verify interactive mode: `dotnet run --project src/ToyRobot` → shows welcome message, accepts typed commands, EXIT quits

### Phase 5: Final Verification

- [ ] Task 5.1: Run full test suite
  - [ ] `dotnet test --verbosity normal` → 71+ tests, all green, zero warnings
- [ ] Task 5.2: Final commit
  - [ ] **Git commit:** `feat: Toy Robot Simulator — complete and passing`

## Dev Notes

### Architecture References

- **Exact code:** Architecture §9 (Simulator), §10 (Program.cs), §11 (GlobalUsings)
- **Simulator rules:** Architecture §9 rules table
- **Test data files:** Architecture §13
- **Integration test cases:** Architecture §12.2 SimulatorTests

### Key Implementation Details

**Simulator** — `src/ToyRobot/Simulation/Simulator.cs`:

```csharp
namespace ToyRobot.Simulation;

public class Simulator(Table table)
{
    private readonly Robot _robot = new();

    public void Execute(ICommand command)
    {
        if (!_robot.IsPlaced && command is not PlaceCommand)
            return;

        command.Execute(_robot, table);
    }
}
```

**GlobalUsings** — `src/ToyRobot/GlobalUsings.cs`:

```csharp
global using ToyRobot.Models;
global using ToyRobot.Commands;
global using ToyRobot.Parsing;
global using ToyRobot.Simulation;
```

**Program.cs** — `src/ToyRobot/Program.cs`:

```csharp
using ToyRobot.Parsing;
using ToyRobot.Simulation;

var table = new Table();
var simulator = new Simulator(table);
var parser = new CommandParser();
var interactive = !Console.IsInputRedirected;

if (interactive)
    Console.WriteLine("Toy Robot Simulator - Type commands or EXIT to quit");

while (Console.ReadLine() is { } input)
{
    if (interactive && input.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
        break;

    if (parser.Parse(input) is { } command)
        simulator.Execute(command);
}
```

### Source Tree (this story adds)

```
src/ToyRobot/
├── GlobalUsings.cs              ← NEW
├── Program.cs                   ← UPDATED (was placeholder)
├── Simulation/
│   └── Simulator.cs             ← NEW
tests/ToyRobot.Tests/
├── Simulation/
│   └── SimulatorTests.cs        ← NEW
test-data/
├── example1.txt                 ← NEW
├── example2.txt                 ← NEW
├── example3.txt                 ← NEW
├── boundary-test.txt            ← NEW
└── invalid-commands.txt         ← NEW
```

### Coding Rules

- R4: Primary constructor for `Simulator`
- R5: Pattern matching — `is not PlaceCommand`, `is { } command`
- R7: No `Console.Write` in domain logic. Only `ReportCommand` and `Program.cs` touch console.
- R12: No unnecessary abstractions. No DI container. Keep it lean.

### Testing

Dev Note: Story Requires the following tests:

- [ ] xUnit Unit Tests: (nextToFile: false), coverage requirement: 100% for Simulator
  - `tests/ToyRobot.Tests/Simulation/SimulatorTests.cs` — write FIRST in Task 1.1

**SimulatorTests (write in Task 1.1 — before Simulator.cs exists):**

> **Important:** These are integration-style tests. Use `StringWriter` to capture stdout for REPORT assertions.
> Each test creates a fresh `Table + Simulator`, runs a command sequence, and asserts output.

| Test Name | Command Sequence | Expected stdout |
|---|---|---|
| `Spec_Example1` | `PLACE 0,0,NORTH` → `MOVE` → `REPORT` | `"0,1,NORTH"` |
| `Spec_Example2` | `PLACE 0,0,NORTH` → `LEFT` → `REPORT` | `"0,0,WEST"` |
| `Spec_Example3` | `PLACE 1,2,EAST` → `MOVE` → `MOVE` → `LEFT` → `MOVE` → `REPORT` | `"3,3,NORTH"` |
| `MoveBeforePlace_Ignored` | `MOVE` → `REPORT` → `PLACE 0,0,NORTH` → `REPORT` | `"0,0,NORTH"` (single line) |
| `PlaceOffTable_RobotNotPlaced` | `PLACE 5,5,NORTH` → `MOVE` → `REPORT` | _(empty — no output)_ |
| `MultiplePlacements_LastWins` | `PLACE 0,0,NORTH` → `PLACE 3,3,SOUTH` → `REPORT` | `"3,3,SOUTH"` |
| `BoundaryProtection_AllEdges` | `PLACE 0,0,SOUTH` → `MOVE` → `REPORT` | `"0,0,SOUTH"` |

> **Test helper pattern:**
> ```csharp
> private static string CaptureOutput(Action action)
> {
>     var sw = new StringWriter();
>     Console.SetOut(sw);
>     action();
>     Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
>     return sw.ToString().Trim();
> }
> ```

- [ ] Manual E2E Tests: (via test data files in Phase 4)
  - 5 test data files piped through `dotnet run`
  - 1 interactive mode check

### Git Commit Cadence

```
1. feat: add Simulator with placement guard (TDD)
2. feat: add GlobalUsings and Program.cs REPL loop
3. test: add test data files for manual E2E validation
4. feat: Toy Robot Simulator — complete and passing
```

Manual Test Steps:

1. `dotnet test --verbosity normal` → 71+ tests, all green, zero warnings
2. `dotnet run --project src/ToyRobot < test-data/example1.txt` → `0,1,NORTH`
3. `dotnet run --project src/ToyRobot < test-data/example2.txt` → `0,0,WEST`
4. `dotnet run --project src/ToyRobot < test-data/example3.txt` → `3,3,NORTH`
5. `dotnet run --project src/ToyRobot < test-data/boundary-test.txt` → 4 lines of output
6. `dotnet run --project src/ToyRobot < test-data/invalid-commands.txt` → 2 lines of output
7. Interactive mode: `dotnet run --project src/ToyRobot` → welcome message, typed commands work, EXIT quits

## Definition of Done

- [ ] Simulator tests written FIRST (all 7 cases), then Simulator implemented
- [ ] All 7 Simulator integration tests pass
- [ ] GlobalUsings.cs created, explicit usings cleaned up
- [ ] Program.cs REPL loop functional
- [ ] All 5 test data files created and validated
- [ ] `dotnet test` — full suite 71+ tests, all green, zero warnings
- [ ] All manual E2E tests produce expected output
- [ ] Git history shows test-first commit for Simulator
