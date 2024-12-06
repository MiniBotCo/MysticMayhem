using Godot;

public partial class Door : Area2D, ISpawnable
{
    private bool _locked = true;
    private AnimatedSprite2D animatedSprite2D;

	[Export]
    public Spawn spawner { get; set; }

    public void Unlock()
    {
        _locked = false;
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Opening");
    }

    public bool IsLocked()
    {
        return _locked;
    }
}
