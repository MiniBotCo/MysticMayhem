using Godot;
using System;

public partial class TrickShot : Area2D
{
    private bool _stuck = false;
	private int _speed = 10;
	private Node2D _stuckBody = null;

    public void Start(Vector2 position, float direction)
	{
		GlobalPosition = position;
		Rotation = direction;
		BodyEntered += OnBodyEntered;
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		
		if (!_stuck)
		{
			Position += new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * _speed;
		}
		else if (_stuckBody != null)
		{
			GlobalPosition = _stuckBody.GlobalPosition + _stuckBody.ToLocal(GlobalPosition);
		}
    }

    public void OnBodyEntered(Node2D body)
	{
		_stuck = true;
		SetDeferred(Area2D.PropertyName.Monitoring, false);
		_stuckBody = body;
	}
}
