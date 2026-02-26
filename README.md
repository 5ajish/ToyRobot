# Toy Robot Simulator

A console application that simulates a toy robot moving on a tabletop grid.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Build

```bash
dotnet build
```

## Run

```bash
# Default 5x5 table
dotnet run --project src/ToyRobot

# Custom table size (width height)
dotnet run --project src/ToyRobot -- 8 8
```

### Piped Mode

```bash
# PowerShell
Get-Content test-data\example1.txt | dotnet run --project src/ToyRobot

# Bash
dotnet run --project src/ToyRobot < test-data/example1.txt
```

## Commands

| Command | Description |
|---------|-------------|
| `PLACE X,Y,F` | Place robot at (X,Y) facing F (NORTH, EAST, SOUTH, WEST) |
| `MOVE` | Move one step in the facing direction |
| `LEFT` | Rotate 90° counter-clockwise |
| `RIGHT` | Rotate 90° clockwise |
| `REPORT` | Print current position and direction |
| `SHOW` | Display the table grid with robot position |
| `EXIT` | Quit (interactive mode only) |

Commands are case-insensitive. The first valid command must be `PLACE`. All commands before a valid `PLACE` are ignored.

## Test

```bash
dotnet test
```

## Examples

```
PLACE 0,0,NORTH
MOVE
REPORT
→ 0,1,NORTH

PLACE 1,2,EAST
MOVE
MOVE
LEFT
MOVE
REPORT
→ 3,3,NORTH

PLACE 2,3,NORTH
SHOW
→
  ┌───┬───┬───┬───┬───┐    Position : (2,3)
4 │   │   │   │   │   │    Facing   : NORTH
  ├───┼───┼───┼───┼───┤
3 │   │   │ ▲ │   │   │        N
  ├───┼───┼───┼───┼───┤        |
2 │   │   │   │   │   │    W --+-- E
  ├───┼───┼───┼───┼───┤        |
1 │   │   │   │   │   │        S
  ├───┼───┼───┼───┼───┤
0 │   │   │   │   │   │
  └───┴───┴───┴───┴───┘
    0   1   2   3   4
```
