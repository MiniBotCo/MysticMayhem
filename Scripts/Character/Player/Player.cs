using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[ExportGroup("Required Nodes")]
	[Export] public AnimationPlayer animationPlayerNode;
	[Export] public Sprite2D sprite2DNode;
	[Export] public StateMachine stateMachineNode;

	public Vector2 direction = new();
	public Vector2 velocity = new();
	public float Speed = 200.0f;
	public float JumpVelocity = -450.0f;

	public override void _Ready()
	{

	}

	public void Flip()
	{
		bool isNotMovingHorizontally = Velocity.X == 0;

		if (isNotMovingHorizontally) { return; }

		bool isMovingLeft = Velocity.X < 0;
		sprite2DNode.FlipH = isMovingLeft;
	}
}
