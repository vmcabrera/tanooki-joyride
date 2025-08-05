
using System;
using System.Collections.Generic;
using Godot;
using TanookiJoyride.Src.CoinScene;
using TanookiJoyride.Src.Common.Components;
using TanookiJoyride.Src.PlayerScene;

namespace TanookiJoyride.Src.Common.Entities;

public partial class EntityManager : Node
{
    public bool UpdateMotionless { get; set; } = true;

    private readonly List<Entity> _entities = [];

    public void AddEntity(Entity entity)
    {
        _entities.Add(entity);
        AddChild(entity);

        ConnectCustomSignals(entity);
    }

    public override void _Process(double delta)
    {
        ForEachSpawnedEntity(entity =>
        {
            if (entity.HasComponent<ScrollingComponent>())
            {
                ScrollingComponent sc = entity.GetComponent<ScrollingComponent>();

                if (UpdateMotionless || sc.HasMotion)
                {
                    entity.GetComponent<ScrollingComponent>().UpdateScrollingPosition(delta);
                }
            }
        });
    }

    public void ClearEntities()
    {
        ForEachSpawnedEntity(entity => RemoveEntity(entity));
    }

    private void ConnectCustomSignals(Entity entity)
    {
        entity.OnRemove += (Entity entity) => RemoveEntity(entity);

        if (entity.HasComponent<CollectibleComponent>()) HandleEntityCollectedSignal(entity);
        if (entity.HasComponent<ObstacleComponent>()) HandleObstacleCollisionSignal(entity);
    }

    private void HandleEntityCollectedSignal(Entity entity)
    {
        switch (entity)
        {
            case CoinGroup coinGroup:
                coinGroup.OnEntityCollected += (Coin coin) => AddCollectedCoin(coin);
                break;
            default:
                GD.PrintErr($"Unhandled collectible entity type: {entity}");
                break;
        }
    }

    private void HandleObstacleCollisionSignal(Entity entity)
    {
        entity.GetComponent<ObstacleComponent>().OnObstacleCollision += (int collisionDamage) => ApplyCollisionDamage(collisionDamage);
    }

    private void AddCollectedCoin(Coin coin)
    {
        GetParent<Main>().AddCollectedCoins(coin.Value);
    }

    private void ApplyCollisionDamage(int collisionDamage)
    {
        GetParent<Main>().GetNode<Player>("Player").TakeDamage(collisionDamage);
    }

    private void RemoveEntity(Entity entity)
    {
        entity.QueueFree();
        _entities.Remove(entity);
    }

    private void ForEachSpawnedEntity(Action<Entity> action)
    {
        for (int i = _entities.Count - 1; i >= 0; i--)
        {
            action(_entities[i]);
        }
    }
}
