using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[ExportGroup("Required Nodes")]
	[Export] public AnimationPlayer animationPlayerNode;
	[Export] public Sprite2D sprite2DNode;
	[Export] public StateMachine stateMachineNode;

	public Vector2 direction = new();
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public override void _Ready()
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		if (Input.IsActionJustPressed(GameConstants.INPUT_ATTACK))
		{
			GD.Print("The player attacked");
			animationPlayerNode.Play(GameConstants.ANIM_ATTACK);
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		direction = Input.GetVector(GameConstants.INPUT_MOVE_LEFT, GameConstants.INPUT_MOVE_RIGHT, GameConstants.INPUT_JUMP, "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;

		MoveAndSlide();
		Flip();
	}

	private void Flip()
	{
		bool isNotMovingHorizontally = Velocity.X == 0;

		if (isNotMovingHorizontally) { return; }

		bool isMovingLeft = Velocity.X < 0;
		sprite2DNode.FlipH = isMovingLeft;
	}
}
