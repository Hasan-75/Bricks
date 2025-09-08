namespace Bricks.Interfacing;

public static class AudioEffects
{
    public static readonly AudioEffect BAR_HIT   = new (800, 100);
    public static readonly AudioEffect LIFE_LOST = new (400, 500);
    public static readonly AudioEffect BRICK_HIT = new (1000,100);
    
    public static readonly List<AudioEffect> GAME_LOST = new()
    {
        new (550, 100),
        new (500, 90),
        new (450, 110),
        new (400, 130),
        new (350, 100),
    };

    public static readonly List<AudioEffect> GAME_WON = new()
    {
        new (800, 100),
        new (900, 90),
        new (1000, 110),
        new (1100, 130),
        new (1200, 100),
    };
}
public record AudioEffect(int Frequency, int Duration);