using Godot;
using Godot.Collections;

public partial class EnemySpawner : Node2D
{
	[Export]
	public int enemyCount { get; set; } = 5;
	[Export]
	public Vector2I levelSize { get; set; } = new Vector2I(60, 40);

	private PackedScene _enemyScene = GD.Load<PackedScene>("res://Scenes/Characters/Character.tscn");
	private Array<CharacterBody2D> _enemyList = new Array<CharacterBody2D>();

    public async void SpawnEnemies(TileMapLayer tileMap)
	{
		for(int i = 0; i < enemyCount; i++)
		{
			CharacterBody2D enemy = _enemyScene.Instantiate<CharacterBody2D>();
			GetTree().Root.CallDeferred(Node.MethodName.AddChild, enemy);
			await ToSignal(enemy, Node.SignalName.Ready);
			PlaceEnemy(tileMap, enemy);
		}
	}

	private void PlaceEnemy(TileMapLayer tileMap, CharacterBody2D enemy)
	{
		Array<Vector2> spawnPositions = GetValidSpawnPositions(tileMap, enemy);

		if (spawnPositions.Count > 0)
		{
			enemy.GlobalPosition = GetValidSpawnPositions(tileMap, enemy).PickRandom();
		}
		else
		{
			enemy.QueueFree();
		}
		
	}
	
	private Array<Vector2> GetValidSpawnPositions(TileMapLayer tileMap, CharacterBody2D enemy)
	{
		Array<Vector2I> tiles = tileMap.GetUsedCells();
		Array<Vector2> validPositions = new Array<Vector2>();
		ShapeCast2D spawnBuffer = enemy.GetNode<ShapeCast2D>("SpawnBuffer");

		for (int i=0; i < tiles.Count; i++)
		{

			spawnBuffer.Position = tileMap.MapToLocal(new Vector2I(tiles[i].X, tiles[i].Y-1)) + new Vector2(0, 2);

			spawnBuffer.ForceUpdateTransform();
			spawnBuffer.ForceShapecastUpdate();

			if (!spawnBuffer.IsColliding())
			{
				validPositions.Add(spawnBuffer.Position);
			}
			
		}

		spawnBuffer.Position = Vector2.Zero;
		return validPositions;
	}
}
