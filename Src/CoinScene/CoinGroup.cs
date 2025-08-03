using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using TanookiJoyride.Src.Common;
using TanookiJoyride.Src.Common.Components;
using TanookiJoyride.Src.Common.Entities;
using TanookiJoyride.Src.Common.Utils;

namespace TanookiJoyride.Src.CoinScene;

public enum CoinGroupPattern
{
    Row,
    Grid,
    Steps,
}

public partial class CoinGroup : Entity
{
    [Signal]
    public delegate void OnEntityCollectedEventHandler(Coin coin);

    private const string CoinScene = "res://Src/CoinScene/Coin.tscn";

    private const int MinPositionHeight = 140;
    private const int MaxPositionHeight = 450;
    private const float CoinMargin = 40f;
    private const float StepsPatternSpawnCooldown = 1f;

    public override void _Ready()
    {
        ScrollingComponent scrollingComponent = AddComponent<ScrollingComponent>(new ScrollingComponent());
        AddChild(scrollingComponent);

        scrollingComponent.SetStartPositionOffset(new Vector2(0, RandomUtility.RandRange(MinPositionHeight, MaxPositionHeight)));
        scrollingComponent.OnScreenExited += OnRemoveEntity;

        IsCollectible = true;

        CreateRandomPattern();
    }

    private void CreateRandomPattern()
    {
        List<CoinGroupPattern> coinGroupPatterns = [.. Enum.GetValues(typeof(CoinGroupPattern)).Cast<CoinGroupPattern>()];
        CoinGroupPattern pattern = coinGroupPatterns[RandomUtility.RandRange(0, coinGroupPatterns.Count - 1)];

        HandlePatterns(pattern);
    }

    private void HandlePatterns(CoinGroupPattern pattern)
    {
        switch (pattern)
        {
            case CoinGroupPattern.Row:
                {
                    CreateRowPattern();
                    break;
                }
            case CoinGroupPattern.Grid:
                {
                    CreateGridPattern();
                    break;
                }
            case CoinGroupPattern.Steps:
                {
                    CreateStepsPattern();
                    AddSpawnCooldownComponent(StepsPatternSpawnCooldown);
                    break;
                }
            default:
                GD.PrintErr($"Unhandled shape type: {pattern}");
                break;
        }
    }

    private void AddSpawnCooldownComponent(float time)
    {
        AddComponent<SpawnCooldownComponent>(new SpawnCooldownComponent(time));
    }

    private void CreateRowPattern()
    {
        const int rowSize = 5;
        const int colSize = 1;

        CreateLinearPattern(new Vector2(rowSize, colSize), (i, j) => new Vector2(CoinMargin * i, CoinMargin * j));
    }

    private void CreateGridPattern()
    {
        const int rowSize = 5;
        const int colSize = 3;

        CreateLinearPattern(new Vector2(rowSize, colSize), (i, j) => new Vector2(CoinMargin * i, CoinMargin * j));
    }

    private void CreateStepsPattern()
    {
        const int rowSize = 5;
        const int colSize = 3;

        CreateLinearPattern(new Vector2(rowSize, colSize), (i, j) => new Vector2((CoinMargin * i) + (CoinMargin * j * rowSize), CoinMargin * j));
    }

    private void CreateLinearPattern(Vector2 length, Func<int, int, Vector2> patternOffset)
    {
        Rect2 notifierRect = new();

        for (int j = 0; j < length.Y; j++)
        {
            for (int i = 0; i < length.X; i++)
            {
                Coin coin = (Coin)ResourceLoader.Load<PackedScene>(CoinScene).Instantiate();
                coin.Position = patternOffset(i, j);

                AddChild(coin);
                coin.OnPlayerEntered += (Coin coin) => OnCoinCollected(coin);

                Rect2 coinRect = coin.GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect();
                coinRect.Position = coin.ToGlobal(coinRect.Position);
                notifierRect = notifierRect.Merge(coinRect);
            }
        }

        GetComponent<ScrollingComponent>().SetNotifierRect(notifierRect);
    }

    private void OnCoinCollected(Coin coin)
    {
        EmitSignal(SignalName.OnEntityCollected, coin);
    }
}
