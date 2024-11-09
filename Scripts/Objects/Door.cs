using Godot;

public partial class Door : Area2D, ISpawnable
{

	[Export]
    public Spawn spawner { get; set; }
}
