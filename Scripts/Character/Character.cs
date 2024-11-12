using Godot;
using System;

public partial class Character : CharacterBody2D
{
    protected Character characterNode;
    // Define gravity and other variables
    [Export] public float Gravity = 900.0f;
    [Export] public float JumpForce = -400.0f;
    [Export] public float MaxFallSpeed = 1500.0f;

    [ExportGroup("Required Nodes")]
    [Export] public AnimationPlayer AnimationPlayerNode { get; private set; }
    [Export] public Sprite2D Sprite2DNode { get; private set; }
    [Export] public StateMachine StateMachineNode { get; private set; }

    [ExportGroup("AI Nodes")]
    [Export] public Path2D PathNode { get; private set; }

    public Vector2 velocity = Vector2.Zero;


    public override void _PhysicsProcess(double delta)
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
