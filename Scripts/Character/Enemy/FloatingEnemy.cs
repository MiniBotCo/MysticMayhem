using Godot;
using System;

public partial class FloatingEnemy : Enemy
{
	// Movement properties
	[Export] public float Speed = 100f;
	[Export] public float FloatAmplitude = 50f;
	[Export] public float FloatFrequency = 2f;

	// Detection properties
	[Export] public float DetectionRange = 200f;
	[Export] public float AttackRange = 50f;

	// Reference to player
	private Node2D _player;

	// Floating logic variables
	private Vector2 _velocity = Vector2.Zero;
	private Vector2 _startPosition;
	private double _floatOffset;

	// State tracking
	private bool _isPlayerDetected = false;
	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		// Store initial position for floating calculation
		_startPosition = Position;
		_floatOffset = 0f;
	}

	public override void _PhysicsProcess(double delta)
	{
		Float(delta);
	}

	private void Float(double delta)
	{
		// Floating behavior using sine wave
		_floatOffset += delta * FloatFrequency;
		double floatY = Mathf.Sin(_floatOffset) * FloatAmplitude;
		float floatYFloat = (float)floatY;
		Position = new Vector2(Position.X, _startPosition.Y + floatYFloat);
	}

}
