using System;
using System.Collections.Generic;
using Godot;

namespace TanookiJoyride.Src.Common.Entities;

public partial class Entity : Node2D
{
    [Signal]
    public delegate void OnRemoveEventHandler(Entity entity);

    public bool IsCollectible { get; protected set; } = false;
    private readonly Dictionary<Type, object> _components = [];

    public T AddComponent<T>(T component)
    {
        _components[typeof(T)] = component;

        return component;
    }

    public bool HasComponent<T>()
    {
        return _components.ContainsKey(typeof(T));
    }

    public T GetComponent<T>()
    {
        if (_components.TryGetValue(typeof(T), out object value))
        {
            return (T)value;
        }

        throw new InvalidOperationException($"Component of type {typeof(T)} not found.");
    }

    public virtual void OnRemoveEntity()
    {
        EmitSignal(SignalName.OnRemove, this);
    }
}


