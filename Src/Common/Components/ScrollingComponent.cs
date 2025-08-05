using System;
using Godot;

namespace TanookiJoyride.Src.Common;

public partial class ScrollingComponent : Node
{
    [Signal]
    public delegate void OnScreenExitedEventHandler();

    public bool HasMotion { get; set; } = false;
    public float Speed { get; set; } = 300f;
    public Vector2 StartPosition { get; set; }
    public int RotationDegrees { get; set; }
    public int RotationSpeed { get; set; }

    private Node2D _parentNode;
    private VisibleOnScreenNotifier2D _notifier;

    private const float StartPositionOffset = 200.0f;

    public override void _Ready()
    {
        _parentNode = GetParent<Node2D>();

        StartPosition = new Vector2(_parentNode.GetViewportRect().Size.X + StartPositionOffset, 0);
        _parentNode.Position = StartPosition;

        _notifier = _parentNode.GetNodeOrNull<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        if (_notifier != null) _notifier.ScreenExited += EmitScreenExitedSignal;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsRotating()) UpdateRotationDegrees(delta);
    }

    public void SetStartPositionOffset(Vector2 positionOffset)
    {
        StartPosition = new Vector2(StartPosition.X + positionOffset.X, StartPosition.Y + positionOffset.Y);
        _parentNode.Position = StartPosition;
    }

    public void SetNotifierRect(Rect2 rect)
    {
        if (_notifier == null) throw new NullReferenceException($"Notifier of {_parentNode} is null.");

        _notifier.Rect = rect;
    }

    public void UpdateScrollingPosition(double delta)
    {
        Vector2 velocity = new(-1, 0);
        velocity = velocity.Normalized() * Speed;

        _parentNode.Position += velocity * (float)delta;
    }

    public bool IsRotating()
    {
        return RotationSpeed > 0;
    }

    public void EmitScreenExitedSignal()
    {
        EmitSignal(SignalName.OnScreenExited);
    }

    private void UpdateRotationDegrees(double delta)
    {
        _parentNode.RotationDegrees += RotationSpeed * (float)delta;

        if (_notifier != null)
        {
            _notifier.GlobalRotation = 0;
        }
    }
}
