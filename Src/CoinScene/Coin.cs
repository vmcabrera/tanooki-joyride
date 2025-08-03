using Godot;
using TanookiJoyride.Src.PlayerScene;

namespace TanookiJoyride.Src.CoinScene;

public partial class Coin : Area2D
{
    [Signal]
    public delegate void OnPlayerEnteredEventHandler(Coin coin);

    public int Value { get; } = 1;

    private void OnBodyEntered(Node2D body)
    {
        if (body.GetType() == typeof(Player))
        {
            EmitSignal(SignalName.OnPlayerEntered, this);
        }

        QueueFree();
    }
}
