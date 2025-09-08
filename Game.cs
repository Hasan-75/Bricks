using System.Text;
using Bricks.Geometry;

namespace Bricks;

public class Game
{
    private bool isRunning;
    private Board board;
    private Bar bar;
    private Ball ball;
    private List<Brick> bricks;

    public Game(Board board, Bar bar, Ball ball)
    {
        this.board = board;
        this.bar   = bar;
        this.ball  = ball;

        this.bricks = CreateBricks(
            brickSize : 2,
            gap       : 2,
            numRows   : 4,
            startRow  : 2,
            boardLeft : board.Start.X + 1,
            boardRight: board.End.X - 1);
    }

    public static Game InitiateGame()
    {
        var board = CreateBoard(size: (50, 25));
        var bar   = CreateBar(initialPosition: (board.Center.X, board.End.Y - 3), length: 9);
        var ball  = CreateBall(initialPosition: bar.CenterPosition - (0, 1), initialVelocity: (2, -1));

        return new Game(board, bar, ball);
    }

    public void Start()
    {
        isRunning = true;
        DrawBoard();
        DrawBar();
        DrawBall();
        DrawBricks();
        Loop();
    }

    private static Board CreateBoard(Vector2 size) => new ()
    {
        Size = size,
    };

    private static Bar CreateBar(Vector2 initialPosition, int length) => new ()
    {
        CenterPosition = initialPosition,
        Length = length,
    };

    private static Ball CreateBall(Vector2 initialPosition, Vector2 initialVelocity) => new ()
    {
        Position = initialPosition,
        Velocity = initialVelocity,
    };

    private void Stop()
    {
        isRunning = false;
    }

    private void Loop()
    {
        var key = ConsoleKey.None;
        Console.CursorVisible = false;

        int frameCounter = 0;

        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                ProcessInput(Console.ReadKey(true).Key);
            }

            if (frameCounter % 5 == 0)
            {
                MoveBall();
            }

            frameCounter = ++frameCounter % 25;
            Thread.Sleep(10);
        }
    }

    private void ProcessInput(ConsoleKey key)
    {
        if (key is ConsoleKey.Q)
        {
            Stop();
        }
        else if (key is ConsoleKey.LeftArrow)
        {
            MoveBarToLeft();
        }
        else if (key is ConsoleKey.RightArrow)
        {
            MoveBarToRight();
        }
    }

    private void MoveBall()
    {
        Console.SetCursorPosition(ball.Position.X, ball.Position.Y);

        // If ball was on the bar, restore bar pixel "="
        var prevChar =
            ball.Position.Y == bar.CenterPosition.Y
            && ball.Position.X >= bar.StartX
            && ball.Position.X <= bar.EndX
                ? Sprites.BAR_PIXEL
                : Sprites.BOARD_EMPTY_SPACE_UNIT_SPRITE;

        Print(prevChar);

        var nextPosition = ball.Position + ball.Velocity;

        var isHittingBar =
            GeometryUtils.DoSegmentsIntersect(
                new(bar.Start, bar.End),
                new(ball.Position, nextPosition));

        var hittingBrick = bricks
            .FirstOrDefault(brick => GeometryUtils.DoSegmentsIntersectWithRectangle(new(ball.Position, nextPosition), brick.Rectangle));

        if (hittingBrick is not null)
        {
            RemoveBrick(hittingBrick);
        }

        if (isHittingBar)
        {
            Console.Beep(800, 100);
        }

        if (isHittingBar || hittingBrick is not null)
        {
            var hitAccuracy = bar.CenterPosition.X - nextPosition.X;

            ball.Velocity.X = hitAccuracy is >= -1 and <= 1 ? 0 : -hitAccuracy / Math.Abs(hitAccuracy);
            ball.Velocity.Y *= -1;
        }
        else
        {
            if (nextPosition.X >= board.End.X || nextPosition.X <= board.Start.X)
            {
                ball.Velocity.X *= -1;
            }

            if (nextPosition.Y >= board.End.Y || nextPosition.Y <= board.Start.Y)
            {
                ball.Velocity.Y *= -1;
            }
        }

        ball.Position += ball.Velocity;
        DrawBall();
    }

    private void MoveBarToLeft()
    {
        if (bar.StartX <= board.Start.X + 2)
        {
            return;
        }

        PrintAt(Sprites.EMPTY_BAR_UNIT_SPRITE, bar.EndX - 1, bar.CenterPosition.Y);
        bar.CenterPosition.X -= 2;
        PrintAt(Sprites.BAR_UNIT_SPRITE, bar.StartX, bar.CenterPosition.Y);
    }

    private void MoveBarToRight()
    {
        if (bar.EndX >= board.End.X - 2)
        {
            return;
        }

        PrintAt(Sprites.EMPTY_BAR_UNIT_SPRITE, bar.StartX, bar.CenterPosition.Y);
        bar.CenterPosition.X += 2;
        PrintAt(Sprites.BAR_UNIT_SPRITE, bar.EndX - 1, bar.CenterPosition.Y);
    }


    private void DrawBall()
    {
        PrintAt(Sprites.BALL_SPRITE, ball.Position.X, ball.Position.Y);
    }

    private void DrawBar()
    {
        var left = bar.CenterPosition.X - (bar.Length / 2);
        var right = bar.CenterPosition.X + (bar.Length / 2);

        Console.SetCursorPosition(bar.CenterPosition.X, bar.CenterPosition.Y);

        while (left <= right)
        {
            Console.CursorLeft = left;
            Print(Sprites.BAR_PIXEL);
            Console.CursorLeft = right;
            Print(Sprites.BAR_PIXEL);
            left++;
            right--;
        }
    }

    private void DrawBoard()
    {
        var row = new StringBuilder();
        for (var r = 0; r <= board.Size.Y; r++)
        {
            for (var c = 0; c <= board.Size.X; c++)
            {
                if (r == board.Start.Y || r >= board.End.Y || c == board.Start.X || c >= board.End.X)
                {
                    row.Append(Sprites.BORDER_UNIT_SPRITE);
                }
                else
                {
                    row.Append(Sprites.BOARD_EMPTY_SPACE_UNIT_SPRITE);
                }
            }

            Print($"{row}\n");

            // Console.Beep(150 * (r + 1), 10); // TODO: Fix the Beep sync
            Thread.Sleep(10);
            row.Clear();
        }
    }

    private static Vector2 Print(string item)
    {
        Console.Write(item);
        return Console.GetCursorPosition();
    }

    private static void PrintAt(string item, int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(item);
    }

    private List<Brick> CreateBricks(int brickSize, int gap, int numRows, int startRow, int boardLeft, int boardRight)
    {
        var bricks = new List<Brick>();

        for (int r = 0; r < numRows; r++)
        {
            int row = startRow + r;

            int offset = (r % 2 == 0) ? 0 : (brickSize + 1) / 2;
            for (int col = boardLeft + offset; col + brickSize - 1 < boardRight; col += brickSize + gap)
            {
                bricks.Add(new Brick
                {
                    Rectangle = new Rectangle(
                        Top   : new((col, row),                 (col + brickSize - 1, row)),
                        Right : new((col + brickSize - 1, row), (col + brickSize - 1, row + 1)),
                        Bottom: new((col, row + 1),             (col + brickSize - 1, row + 1)),
                        Left  : new((col, row),                 (col, row + 1))
                    ),
                });
            }
        }

        return bricks;
    }

    private void DrawBricks()
    {
        foreach (var brick in bricks)
        {
            for (int i = 0; i < brick.Length; i++)
            {
                int x = (int)brick.Rectangle.Top.Start.X + i;
                int y = (int)brick.Rectangle.Top.Start.Y;
                PrintAt(Sprites.BRICK_UNIT_SPRITE, x, y);
            }
        }
    }

    private void RemoveBrick(Brick brick)
    {
        bricks.Remove(brick);

        for (int i = 0; i < brick.Length; i++)
        {
            int x = (int)brick.Rectangle.Top.Start.X + i;
            int y = (int)brick.Rectangle.Top.Start.Y;
            PrintAt(Sprites.BOARD_EMPTY_SPACE_UNIT_SPRITE, x, y);
        }

        Console.Beep(1000, 100);
    }
}