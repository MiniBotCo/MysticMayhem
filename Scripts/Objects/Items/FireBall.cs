using Godot;
using System;

public partial class FireBall : RigidBody2D
{
    [Export] private float Speed = 350;
    private Timer _timer;

    public void Start(Vector2 position, float direction)
    {
        _timer = GetNode<Timer>("Lifetime");
        GlobalPosition = position;
        LinearVelocity = Speed * new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));

        _timer.Timeout += () => QueueFree();

    }

    public override void _PhysicsProcess(double delta)
    {

        base._PhysicsProcess(delta);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        GetNode<Sprite2D>("Sprite2D").RotationDegrees += 5;
    }
}