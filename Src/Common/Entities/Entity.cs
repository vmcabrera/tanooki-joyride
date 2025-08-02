using Godot;
using System;
using System.Collections.Generic;

namespace TanookiJoyride.Src.Common.Entities;

public partial class Entity : Node2D
{
    [Signal]
    public delegate void OnRemoveEventHandler();

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
        if (_components.TryGetValue(typeof(T), out var value))
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


