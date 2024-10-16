using Godot;
using System;

public partial class Character : CharacterBody2D
{
    // Define gravity and other variables
    [Export] public float Gravity = 900.0f;
    [Export] public float JumpForce = -400.0f;
    [Export] public float MaxFallSpeed = 1500.0f;

    private Vector2 velocity = Vector2.Zero;

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
    }
}
