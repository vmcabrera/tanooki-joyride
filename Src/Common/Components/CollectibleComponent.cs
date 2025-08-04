using Godot;
using TanookiJoyride.Src.Common.Entities;

namespace TanookiJoyride.Src.Common.Components;

public partial class CollectibleComponent(Entity entity) : Node
{
    [Signal]
    public delegate void OnEntityCollectedEventHandler(Entity entity);

    public Entity Entity { get; private set; } = entity;

    public void EmitOnEntityCollected()
    {
        EmitSignal(SignalName.OnEntityCollected, Entity);
    }
}
