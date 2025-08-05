using Godot;

namespace TanookiJoyride.Src.Common.Components;
public partial class ObstacleComponent() : Node2D
{
    [Signal]
    public delegate void OnObstacleCollisionEventHandler(int collisionDamage);

    public int CollisionDamage { get; set; }

    public void EmitObstacleCollision()
    {
        EmitSignal(SignalName.OnObstacleCollision, CollisionDamage);
    }
}
