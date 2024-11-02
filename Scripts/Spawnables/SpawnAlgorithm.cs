using Godot;
using System;

public partial class SpawnAlgorithm : Node
{
	[Export]
	public SpawnerAlgorithm spawnerAlgorithm{get; set; } = new SpawnerAlgorithm();
}
