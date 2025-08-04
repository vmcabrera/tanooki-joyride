using Godot;
using TanookiJoyride.Src.Common.Components;
using TanookiJoyride.Src.Common.Entities;
using TanookiJoyride.Src.PlayerScene;

namespace TanookiJoyride.Src.CoinScene;

public partial class Coin : Entity
{
    public int Value { get; } = 1;

    public override void _Ready()
    {
        AddComponent<CollectibleComponent>(new CollectibleComponent(this));
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.GetType() == typeof(Player))
        {
            GetComponent<CollectibleComponent>().EmitOnEntityCollected();
        }

        QueueFree();
    }
}
