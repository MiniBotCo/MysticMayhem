using Godot;
using System;

public partial class Player : Character
{
	public Vector2 direction = new();
	[Export(PropertyHint.Range, "50,800,25")] public float PlayerSpeed = 200.0f;
	[Export(PropertyHint.Range, "-850,-50,25")] public float JumpVelocity = -450.0f;

	public override void _Ready()
	{
		characterNode = GetOwner<Player>();
	}


}
