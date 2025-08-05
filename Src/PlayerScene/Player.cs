using Godot;

namespace TanookiJoyride.Src.PlayerScene;

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void OnDeathEventHandler();

    private AnimatedSprite2D _playerAnimatedSprite;

    public int Health { get; private set; } = 1;

    public const float Speed = 300.0f;
    public const float JumpVelocity = -300.0f;
    public const int StartingHealth = 1;

    private const int DeathAngle = 90;

    public override void _Ready()
    {
        _playerAnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        if (IsAlive())
        {
            velocity = HandlePlayerInputs(velocity);
        }
        else
        {
            HandleDeathAnimation(velocity);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public void Reset()
    {
        Health = StartingHealth;
        RotationDegrees = 0;
    }

    private Vector2 HandlePlayerInputs(Vector2 velocity)
    {
        if (Input.IsActionPressed("use_jetpack"))
        {
            velocity.Y = JumpVelocity;
            _playerAnimatedSprite.Animation = "flying";
        }
        else
        {
            if (IsOnFloor())
            {
                _playerAnimatedSprite.Animation = "walking";
            }
            else
            {
                _playerAnimatedSprite.Animation = "falling";
            }
        }

        return velocity;
    }

    private void HandleDeathAnimation(Vector2 velocity)
    {
        _playerAnimatedSprite.Animation = "dying";
        if (RotationDegrees < DeathAngle) RotationDegrees += 5;

        if (velocity.Y == 0)
        {
            _playerAnimatedSprite.Animation = "dead";
            EmitSignal(SignalName.OnDeath);
        }
    }
}
