using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	[ExportGroup("Required Nodes")]
	[Export] public AnimationPlayer animationPlayerNode;
	[Export] public Sprite2D sprite2DNode;
	[Export] public StateMachine stateMachineNode;

	[ExportGroup("Player Stats")]
	[Export] public float Speed = 500.0f;
	[Export] public float JumpVelocity = -700.0f;
	[Export] public float Health = 20.0f;
	[Export] public float Damage = 5.0f;

	public Vector2 direction = new();


	private static List<Effect> _effects = new List<Effect>();

	public override void _Ready()
	{
		ApplyEffects();
	}

	public override void _PhysicsProcess(double delta)
	{
		//TODO remove
		if (Input.IsActionJustPressed("Debug"))
		{
			_effects.Add(new Effect("health", 1.0f, true));
		}


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

	private void ApplyEffects()
	{
		GD.Print("Effects: "); //TODO remove

		List<Effect> nonPermanentEffects = new List<Effect>();
		foreach (Effect effect in _effects)
		{
			switch(effect.name)
			{
				case "health":
					Health += effect.amount;
					break;
				case "damage":
					Damage += effect.amount;
					break;
				case "speed":
					Speed += effect.amount;
					break;
				case "jump":
					JumpVelocity -= effect.amount;
					break;
				default:
					break;
			}

			if (!effect.permanent)
			{
				nonPermanentEffects.Add(effect);
			}

			GD.Print(effect.name); //TODO remove
		}

		foreach (Effect effect in nonPermanentEffects)
		{
			if (!effect.permanent) _effects.Remove(effect);
		}

		GD.Print("Stats: Health " + Health + " | Damage " + Damage + " | Speed " + Speed + " | Jump Speed " + JumpVelocity); //TODO remove
	}
}
