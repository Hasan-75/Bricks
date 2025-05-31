using System.Text;

namespace Bricks;

public static class Game
{
    private static bool IsRunning;
    private static Board Board;
    private static Bar Bar;

    public static void Start()
    {
        InitialSetup();
        DrawBoard();
        DrawBar();
        StartLoop();
    }

    public static void Stop()
    {
        IsRunning = false;
    }

    public static void StartLoop()
    {
        var key = ConsoleKey.None;
        Console.CursorVisible = false;

        while (IsRunning)
        {
            // INPUT
            if (Console.KeyAvailable)
            {
                ProcessInput(Console.ReadKey(true).Key);
            }

            Thread.Sleep(50); // ~5 FPS
        }
    }

    private static void ProcessInput(ConsoleKey key)
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

    private static void MoveBarToLeft()
    {
        Console.CursorLeft = Bar.BarEndX;
        Print(" ");
        Bar.CenterPosition.X--;
        Console.CursorLeft = Bar.BarStartX;
        Print("=");
    }

    private static void MoveBarToRight()
    {

        Console.CursorLeft = Bar.BarStartX;
        Print(" ");
        Bar.CenterPosition.X++;
        Console.CursorLeft = Bar.BarEndX;
        Print("=");
    }

    private static void DrawBar()
    {
        var left = Bar.CenterPosition.X - (Bar.Length / 2);
        var right = Bar.CenterPosition.X + (Bar.Length / 2);

        Console.SetCursorPosition(Bar.CenterPosition.X, Bar.CenterPosition.Y);

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

    private static void DrawBoard()
    {
        var row = new StringBuilder();
        for (var r = 0; r <= Board.Size; r++)
        {
            for (var c = 0; c <= Board.Size; c++)
            {
                if (r < Board.BoardStart || r >= Board.BoardEnd || c < Board.BoardStart || c >= Board.BoardEnd)
                {
                    row.Append("**");
                }
                else
                {
                    row.Append("  ");
                }
            }
            Print($"{row}\n");
            Console.Beep(150 * (r + 1), 50); // TODO: Fix the Beep sync
            Thread.Sleep(50);
            row.Clear();
        }
    }

    private static void InitialSetup()
    {
        IsRunning = true;

        Board = new Board
        {
            Size = 20,
        };

        Bar = new Bar
        {
            CenterPosition = (Board.Center.X, Board.BoardEnd),
            Length = 5,
        };
    }

    private static Vector2 Print(string item)
    {
        Console.Write(item);
        return Console.GetCursorPosition();
    }
}