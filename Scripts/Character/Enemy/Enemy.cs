using Godot;
using System;

public partial class Enemy : Character
{
	[ExportGroup("Required Nodes")]
	[Export] public AnimationPlayer AnimationPlayerNode { get; private set; }
	[Export] public Sprite2D Sprite2DNode { get; private set; }
	[Export] public StateMachine StateMachineNode { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
