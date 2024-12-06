using Godot;
using System;

public partial class FireBall : RigidBody2D
{
    [Export] private float Speed = 350;
    private Timer _timer;
    private Area2D _hitBox;

    private float damage = 10;

    public void Start(Vector2 position, float direction, float damage)
    {
        this.damage = damage;

        _timer = GetNode<Timer>("Lifetime");
        _hitBox = GetNode<Area2D>("HitBox");
        GlobalPosition = position;
        LinearVelocity = Speed * new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));

        _timer.Timeout += () => QueueFree();
        _hitBox.BodyEntered += OnHitBoxEntered;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        GetNode<Sprite2D>("Sprite2D").RotationDegrees += 5;
    }

    private async void OnHitBoxEntered(Node2D body)
    {
        GetNode<Sprite2D>("Sprite2D").Visible = false;
        GetNode<CpuParticles2D>("ExplosionParticles").Emitting = true;

        await ToSignal(GetNode<CpuParticles2D>("ExplosionParticles"), CpuParticles2D.SignalName.Finished);

        if (body is Character)
        {
            ((Character) body).GetStatResource(Stat.Health).StatValue -= damage;
        }

        QueueFree();
    }
}