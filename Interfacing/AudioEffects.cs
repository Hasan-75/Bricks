namespace Bricks.Interfacing;

public static class AudioEffects
{
    public static readonly AudioEffect BAR_HIT   = new (800, 100);
    public static readonly AudioEffect LIFE_LOST = new (400, 500);
    public static readonly AudioEffect BRICK_HIT = new (1000,100);
    
    public static readonly List<AudioEffect> GAME_LOST = new()
    {
        new (600, 150),
        new (550, 120),
        new (500, 100),
        new (400, 180),
        new (300, 100)
    };

    public static readonly List<AudioEffect> GAME_WON = new()
    {
        new (700, 80),
        new (900, 80),
        new (1100, 80),
        new (1200, 120),
        new (1400, 150),
        new (1600, 200)
    };
}
public record AudioEffect(int Frequency, int Duration);