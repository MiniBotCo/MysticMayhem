using Godot;

public class FireBallAbility : Effect, IAbility
{
    public FireBallAbility(string name, float amount, bool permanent = false) : base(name, amount, permanent)
    {
    }

    public async void Trigger(Node2D parent)
    {
        PackedScene fireBallScene = GD.Load<PackedScene>("res://Scenes/Objects/Items/fire_ball.tscn");
        FireBall fireBall = fireBallScene.Instantiate<FireBall>();
        parent.GetTree().Root.CallDeferred(Node2D.MethodName.AddChild, fireBall);

        await parent.ToSignal(fireBall, Node.SignalName.Ready);

        GD.Print(parent.GetAngleTo(parent.GetGlobalMousePosition()));
        fireBall.Start(parent.GlobalPosition, parent.GetAngleTo(parent.GetGlobalMousePosition()));
    }
}