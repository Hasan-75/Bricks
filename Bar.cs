namespace Bricks;

public class Bar
{
    public required int Length { get; set; }

    public required Vector2 CenterPosition { get; set; }

    public int EndX => CenterPosition.X + (Length / 2);

    public int StartX => CenterPosition.X - (Length / 2);
}
