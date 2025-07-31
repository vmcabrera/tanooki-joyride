using Godot;
using TanookiJoyride.Src.HudScene;

namespace TanookiJoyride.Src;

public partial class Main : Node
{
    private Hud _hud;
    private ParallaxBackground _background;

    private int _currentDistance = 0;
    private int _bestDistance = 0;
    private int _currentCoins = 0;

    private const float _scrollingSpeed = 150f;
    private const int PixelsPerMeter = 20;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _hud = GetNode<Hud>("Hud");
        _hud.Connect("OnResetGame", new Callable(this, MethodName.ResetGame));

        _background = GetNode<ParallaxBackground>("ParallaxBackground");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        _background.ScrollOffset = new Vector2(_background.ScrollOffset.X - (_scrollingSpeed * (float)delta), 0);

        UpdateCurrentDistance(delta);
    }

    private void UpdateCurrentDistance(double delta)
    {
        _currentDistance += (int)(_scrollingSpeed * delta);
        _hud.UpdateCurrentDistance(_currentDistance / PixelsPerMeter);
    }

    private void ResetGame()
    {
        CheckAndUpdateBestDistance();

        _currentDistance = 0;
        _currentCoins = 0;

        _hud.UpdateCurrentDistance(_currentDistance);
        _hud.UpdateCurrentCoins(_currentCoins);

    }

    private void CheckAndUpdateBestDistance()
    {
        if (_currentDistance > _bestDistance)
        {
            _bestDistance = _currentDistance;
            _hud.UpdateBestDistance(_bestDistance / PixelsPerMeter);
        }
    }
}
