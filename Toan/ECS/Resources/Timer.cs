using Microsoft.Xna.Framework;

namespace Toan.ECS.Resources;

public class Timer : Resource
{
    public required double Interval { get; set; }

    public bool IsCompleted => Completed > 0;
    public int Completed => MathUtil.FloorToInt(_elapsed / Interval);
    public bool JustCompleted { get; private set; }

    private double _elapsed = 0.0;

    public void Tick(GameTime gameTime)
    {
        bool checkComplete = !IsCompleted;
        _elapsed += gameTime.ElapsedGameTime.TotalSeconds;
        if (checkComplete && IsCompleted) JustCompleted = true;
        else if (IsCompleted) JustCompleted = false;
    }

    public void Reset()
    {
        _elapsed = 0.0;
        JustCompleted = false;
    }
}
