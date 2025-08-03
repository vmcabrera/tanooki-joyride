using Godot;
using TanookiJoyride.Src.Common;
using TanookiJoyride.Src.Common.Entities;
using TanookiJoyride.Src.Common.Utils;

namespace TanookiJoyride.Src.ZapperScene;

public partial class Zapper : Entity
{
    private Sprite2D _leftSprite;
    private Sprite2D _middleSprite;
    private Sprite2D _rightSprite;

    private const int MinPositionHeight = 225;
    private const int MaxPositionHeight = 400;

    private const int MinLengthMultiplier = 3;
    private const int MaxLengthMultiplier = 6;

    private const int MaxRotationAngleMultiplier = 3;
    private const float RotationAngleOffset = 45f;

    private const int RotationSpeed = 15;
    private const int MaxRotationSpeedMultiplier = 3;
    private const float RotationSpeedChance = 0.5f;

    public override void _Ready()
    {
        _leftSprite = GetNode<Sprite2D>("LeftSprite2D");
        _middleSprite = GetNode<Sprite2D>("MiddleSprite2D");
        _rightSprite = GetNode<Sprite2D>("RightSprite2D");

        ScrollingComponent scrollingComponent = AddComponent<ScrollingComponent>(new ScrollingComponent());
        AddChild(scrollingComponent);

        scrollingComponent.SetStartPositionOffset(new Vector2(0, RandomUtility.RandRange(MinPositionHeight, MaxPositionHeight)));
        scrollingComponent.OnScreenExited += OnRemoveEntity;

        SetScrollingRotation(scrollingComponent);
        RandomizeLength();
    }

    private void SetScrollingRotation(ScrollingComponent scrollingComponent)
    {
        RotationDegrees = RandomUtility.RandRange(0, MaxRotationAngleMultiplier) * RotationAngleOffset;

        if (RandomUtility.Randf() < RotationSpeedChance)
        {
            scrollingComponent.RotationSpeed = RandomUtility.RandRange(0, MaxRotationSpeedMultiplier) * RotationSpeed;
        }
    }

    private void RandomizeLength()
    {
        int lengthMultiplier = RandomUtility.RandRange(MinLengthMultiplier, MaxLengthMultiplier);

        Vector2 middleTileSize = _middleSprite.Texture.GetSize();
        float middleSpriteWidth = middleTileSize.X * lengthMultiplier;
        _middleSprite.RegionRect = new Rect2(0, 0, middleSpriteWidth, middleTileSize.Y);

        UpdateSideSpriteOffsets(middleSpriteWidth, middleTileSize);
        UpdateBoundaries(middleSpriteWidth, middleTileSize);
    }

    private void UpdateSideSpriteOffsets(float width, Vector2 tileSize)
    {
        float sideSpriteOffset = (width - tileSize.X) / 2f;

        _leftSprite.Offset = new Vector2(-sideSpriteOffset, 0);
        _rightSprite.Offset = new Vector2(sideSpriteOffset, 0);
    }

    private void UpdateBoundaries(float width, Vector2 tileSize)
    {
        Vector2 middleSpritePosition = new(-width / 2f - tileSize.X * 0.5f, -tileSize.Y * 0.5f);
        Rect2 spriteRect = new(new Vector2(middleSpritePosition.X, middleSpritePosition.Y), new Vector2(width + tileSize.X * 2, tileSize.Y));

        GetComponent<ScrollingComponent>().SetNotifierRect(spriteRect);

        RectangleShape2D shape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;
        shape.Size = new Vector2(spriteRect.Size.X, spriteRect.Size.Y);
    }
}
