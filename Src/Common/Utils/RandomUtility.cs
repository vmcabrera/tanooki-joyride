using Godot;

namespace TanookiJoyride.Src.Common.Utils;
public static class RandomUtility
{
    private static readonly RandomNumberGenerator s_rng;

    static RandomUtility()
    {
        s_rng = new RandomNumberGenerator();
        s_rng.Randomize();
    }

    public static int RandRange(int min, int max) => s_rng.RandiRange(min, max);
    public static float Randf() => s_rng.Randf();
}
