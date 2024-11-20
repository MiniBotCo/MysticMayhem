using Godot;
using System.Collections.Generic;

public partial class EnemyGenerator : Node2D
{
	[Export] public PackedScene enemyScene = GD.Load<PackedScene>("res://Scenes/Characters/Enemy/Enemy.tscn");

	[Signal] public delegate void EnemiesGeneratedEventHandler();
	
	public async void GenerateEnemies(int level, Spawner spawner)
	{
		for (int i = 0; i < (level - 1)%5 + 1; i++)
		{
			Enemy enemy = enemyScene.Instantiate<Enemy>();
			CallDeferred(Node.MethodName.AddChild, enemy);
			await ToSignal(enemy, Node.SignalName.Ready);

			spawner.AddToSpawnList(enemy);
		}
		EmitSignal(SignalName.EnemiesGenerated);
	}
}
