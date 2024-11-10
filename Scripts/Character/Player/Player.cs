using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[ExportGroup("Required Nodes")]
	[Export] public AnimationPlayer AnimationPlayerNode { get; private set; }
	[Export] public Sprite2D Sprite2DNode { get; private set; }
	[Export] public StateMachine StateMachineNode { get; private set; }

	public Vector2 direction = new();
	public Vector2 velocity = new();
	[Export(PropertyHint.Range, "50,800,25")] public float PlayerSpeed = 200.0f;
	[Export(PropertyHint.Range, "-850,-50,25")] public float JumpVelocity = -450.0f;

	public override void _Ready()
	{

	}

	public void Flip()
	{
		bool isNotMovingHorizontally = Velocity.X == 0;

		if (isNotMovingHorizontally) { return; }

		bool isMovingLeft = Velocity.X < 0;
		Sprite2DNode.FlipH = isMovingLeft;
	}
}
