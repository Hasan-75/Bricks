using Bricks.Geometry;

namespace Bricks;

public class Brick
{
    public required Rectangle Rectangle { get; set; }

    public int Length => Rectangle.Top.End.X - Rectangle.Top.Start.X + 1;
}