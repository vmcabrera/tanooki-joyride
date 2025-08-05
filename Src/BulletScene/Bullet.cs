using Godot;
using TanookiJoyride.Src.Common;
using TanookiJoyride.Src.Common.Components;
using TanookiJoyride.Src.Common.Entities;
using TanookiJoyride.Src.Common.Utils;
using TanookiJoyride.Src.PlayerScene;

namespace TanookiJoyride.Src.BulletScene;

public partial class Bullet : Entity
{
    private const int CollisionDamage = 1;
    private const int MinPositionHeight = 150;
    private const int MaxPositionHeight = 475;
    private const float Speed = 750f;

    public override void _Ready()
    {
        ScrollingComponent scrollingComponent = AddComponent<ScrollingComponent>(new ScrollingComponent());
        AddChild(scrollingComponent);

        scrollingComponent.HasMotion = true;
        scrollingComponent.Speed = Speed;
        scrollingComponent.SetStartPositionOffset(new Vector2(0, RandomUtility.RandRange(MinPositionHeight, MaxPositionHeight)));
        scrollingComponent.OnScreenExited += OnRemoveEntity;

        ObstacleComponent obstacleComponent = AddComponent<ObstacleComponent>(new ObstacleComponent());
        AddChild(obstacleComponent);

        obstacleComponent.CollisionDamage = CollisionDamage;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.GetType() == typeof(Player))
        {
            GetComponent<ObstacleComponent>().EmitObstacleCollision();
        }
    }
}
