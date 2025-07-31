using Godot;

namespace TanookiJoyride.Src.HudScene;

public partial class Hud : CanvasLayer
{
    [Signal]
    public delegate void OnResetGameEventHandler();

    private Label _currentDistanceLabel;
    private Label _bestDistanceLabel;
    private Label _currentCoinsLabel;

    public override void _Ready()
    {
        _currentDistanceLabel = GetNode<Label>("%CurrentDistanceLabel");
        _bestDistanceLabel = GetNode<Label>("%BestDistanceLabel");
        _currentCoinsLabel = GetNode<Label>("%CurrentCoinsLabel");

        GetNode<Button>("%ResetButton").Connect("pressed", new Callable(this, MethodName.OnResetButtonPressed));
    }

    public void UpdateCurrentDistance(int distance)
    {
        _currentDistanceLabel.Text = $"{distance}M";
    }

    public void UpdateBestDistance(int distance)
    {
        _bestDistanceLabel.Text = $"BEST: {distance}M";
    }

    public void UpdateCurrentCoins(int amount)
    {
        _currentCoinsLabel.Text = $"{amount}$";
    }

    private void OnResetButtonPressed()
    {
        EmitSignal(SignalName.OnResetGame);
    }
}
