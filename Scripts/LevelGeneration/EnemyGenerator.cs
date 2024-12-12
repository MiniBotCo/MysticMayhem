using Godot;
using System.Collections.Generic;

public partial class EnemyGenerator : Node2D
{
	[Export] public PackedScene enemyScene = GD.Load<PackedScene>("res://Scenes/Characters/Enemy/Enemy.tscn");

	[Signal] public delegate void EnemiesGeneratedEventHandler();
	[Signal] public delegate void EnemiesDefeatedEventHandler();

	private int enemies = 0;
	
	public async void GenerateEnemies(int level, Spawner spawner)
	{
		for (int i = 0; i < (level - 1)%5 + 1; i++)
		{
			Enemy enemy = enemyScene.Instantiate<Enemy>();
			CallDeferred(Node.MethodName.AddChild, enemy);
			await ToSignal(enemy, Node.SignalName.Ready);

			enemies++;
			enemy.Death += OnEnemyKilled;
			spawner.AddToSpawnList(enemy);
		}
		EmitSignal(SignalName.EnemiesGenerated);
	}
	private void OnEnemyKilled()
	{
		enemies--;
		GameStatistics.totalEnemiesDefeated++;
		GameStatistics.enemiesDefeated++;

		if (enemies <= 0)
		{
			EmitSignal(SignalName.EnemiesDefeated);
		}
	}
}
