using Godot;
using System;

public partial class TrickShot : Area2D
{
	private int _speed = 10;
	private float _damage = 10;

    public void Start(Vector2 position, float direction, float damage)
	{
		GlobalPosition = position;
		Rotation = direction;
		_damage = damage;
		BodyEntered += OnBodyEntered;
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		Position += new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * _speed;
    }

    public void OnBodyEntered(Node2D body)
	{
		if (body is Character)
        {
            ((Character) body).GetStatResource(Stat.Health).StatValue -= _damage;
        }

		QueueFree();
	}
}
