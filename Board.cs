
namespace Bricks;

public class Board
{
    public required int Size { get; set; }

    public int BoardStart => 1;

    public int BoardEnd => Size;

    public Vector2 Center => (Size/2, Size/2);
}
public class Bar
{
    public required int Length { get; set; }

    public required Vector2 CenterPosition { get; set; }

    public int BarEndX => CenterPosition.X + (Length / 2);

    public int BarStartX => CenterPosition.X - (Length / 2);
}

public record Vector2
{
    public int X { get; set; }

    public int Y { get; set; }

    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static implicit operator Vector2((int, int)tuple) => new Vector2(tuple.Item1, tuple.Item2);
}