namespace Bricks.Geometry;

public record Vector2
{
    public int X { get; set; }

    public int Y { get; set; }

    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }


    public int Cross(Vector2 target) => (X * target.Y) - (Y * target.X);

    public int Dot(Vector2 target)   => (X * target.X) + (Y * target.Y);


    public static implicit operator Vector2((int, int) tuple) => new Vector2(tuple.Item1, tuple.Item2);

    public static Vector2 operator +(Vector2 v1, Vector2 v2)  => (v1.X + v2.X, v1.Y + v2.Y);

    public static Vector2 operator -(Vector2 v1, Vector2 v2)  => (v1.X - v2.X, v1.Y - v2.Y);
}