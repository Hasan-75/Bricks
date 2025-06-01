using System.Text;

namespace Bricks;

public class Game
{
    private bool isRunning;
    private Board board;
    private Bar bar;
    private Ball ball;

    public Game(Board board, Bar bar, Ball ball)
    {
        this.board = board;
        this.bar = bar;
        this.ball = ball;
    }

    public static Game InitiateGame()
    {
        var board = CreateBoard(size: (50, 25));
        var bar = CreateBar(initialPosition: (board.Center.X, board.End.Y - 3), length: 7);
        var ball = CreateBall(initialPosition: bar.CenterPosition - (0, 1), initialVelocity: (2, -1));

        return new Game(board, bar, ball);
    }

    public void Start()
    {
        isRunning = true;
        DrawBoard();
        DrawBar();
        DrawBall();
        StartLoop();
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

    private void StartLoop()
    {
        var key = ConsoleKey.None;
        Console.CursorVisible = false;

        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                ProcessInput(Console.ReadKey(true).Key);
            }

            MoveBall();

            Thread.Sleep(50); // ~5 FPS
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
        Print(" ");

        var nextPosition = ball.Position + ball.Velocity;

        if (nextPosition.Y == bar.CenterPosition.Y && nextPosition.X >= bar.StartX && nextPosition.X <= bar.EndX)
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

        Console.SetCursorPosition(bar.CenterPosition.X, bar.CenterPosition.Y);
        Console.CursorLeft = bar.EndX;
        Print("  ");
        bar.CenterPosition.X -= 2;
        Console.CursorLeft = bar.StartX;
        Print("==");
    }

    private void MoveBarToRight()
    {
        if (bar.EndX >= board.End.X - 2)
        {
            return;
        }

        Console.SetCursorPosition(bar.CenterPosition.X, bar.CenterPosition.Y);
        Console.CursorLeft = bar.StartX;
        Print("  ");
        bar.CenterPosition.X += 2;
        Console.CursorLeft = bar.EndX;
        Print("==");
    }

    public void DrawBall()
    {
        Console.SetCursorPosition(ball.Position.X, ball.Position.Y);
        Console.Write("o");
    }

    private void DrawBar()
    {
        var left = bar.CenterPosition.X - (bar.Length / 2);
        var right = bar.CenterPosition.X + (bar.Length / 2);

        Console.SetCursorPosition(bar.CenterPosition.X, bar.CenterPosition.Y);

        while (left <= right)
        {
            Console.CursorLeft = left;
            Print("=");
            Console.CursorLeft = right;
            Print("=");
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
                    row.Append("*");
                }
                else
                {
                    row.Append(" ");
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
}