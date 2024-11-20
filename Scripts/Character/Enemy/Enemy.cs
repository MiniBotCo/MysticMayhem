using Godot;
using System;

public partial class Enemy : Character, ISpawnable
{
	[Export(PropertyHint.Range, "50,800,25")] public float EnemySpeed = 150.0f;
	[Export]
	public Spawn spawner { get; set; }

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		// Apply gravity
		velocity.Y += Gravity * (float)delta;

		// Clamp the falling speed to prevent infinite fall speed
		if (velocity.Y > MaxFallSpeed)
		{
			velocity.Y = MaxFallSpeed;
		}

		Velocity = velocity;
		// Move and slide the character using its velocity
		MoveAndSlide();
		Velocity = velocity;

	}
}
