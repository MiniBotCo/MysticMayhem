using Godot;
using System;

public partial class Door : AnimatedSprite2D
{
	[Export]
	PackedScene player { get; set; } = new PackedScene();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
}
