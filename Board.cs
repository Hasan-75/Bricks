namespace Bricks;

public class Board
{
    public required Vector2 Size { get; set; }

    public Vector2 Start => (0, 0);

    public Vector2 End => Size;

    public Vector2 Center => (Size.X / 2, Size.Y / 2);
}
