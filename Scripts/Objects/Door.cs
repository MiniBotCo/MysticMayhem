using Godot;
using System;

public partial class Door : AnimatedSprite2D
{
	[Export]
	PackedScene player { get; set; } = new PackedScene();
}
