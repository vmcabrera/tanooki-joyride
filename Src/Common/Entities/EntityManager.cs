using System;
using System.Collections.Generic;
using Godot;

namespace TanookiJoyride.Src.Common.Entities;

public partial class EntityManager : Node
{
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
                entity.GetComponent<ScrollingComponent>().UpdateScrollingPosition(delta);
            }
        });
    }

    private void ConnectCustomSignals(Entity entity)
    {
        entity.OnRemove += (Entity entity) => RemoveEntity(entity);

        if (entity.IsCollectible) HandleEntityCollectedSignal(entity);
    }

    private void HandleEntityCollectedSignal(Entity entity)
    {
        switch (entity)
        {
            default:
                break;
        }
    }

    public void ClearEntities()
    {
        ForEachSpawnedEntity(entity => RemoveEntity(entity));
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
