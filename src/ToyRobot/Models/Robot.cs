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
