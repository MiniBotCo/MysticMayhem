using Godot;
using Godot.Collections;

public partial class Spawner : Node2D
{
	[Export]
	public int spawnCount { get; set; } = 5;
	[Export]
	public Vector2I levelSize { get; set; } = new Vector2I(60, 40);

	private PackedScene _spawnScene = GD.Load<PackedScene>("res://Scenes/Characters/Character.tscn");
	private Array<Node2D> _spawnList = new Array<Node2D>();

	/// <summary>
	/// Spawns the spawnable on the tilemap passed. Currently intended only for use with the platform tilemap
	/// </summary>
	/// <param name="tileMap">The tilemap to spawn spawnables on</param>
    public async void Spawn(TileMapLayer tileMap)
	{
		for(int i = 0; i < spawnCount; i++)
		{
			Node2D spawn = _spawnScene.Instantiate<Node2D>();
			if (spawn.IsInGroup("Spawnable"))
			{
				GetTree().Root.CallDeferred(Node.MethodName.AddChild, spawn);
				await ToSignal(spawn, Node.SignalName.Ready);
				PlaceSpawn(tileMap, spawn);
			}
		}
	}

	/// <summary>
	/// Places a spawnable at a valid position on the given tilemap
	/// </summary>
	/// <param name="tileMap">The tilemap to place on</param>
	/// <param name="spawn">The spawnable to be placed</param>
	private void PlaceSpawn(TileMapLayer tileMap, Node2D spawn)
	{
		Array<Vector2> spawnPositions = GetValidSpawnPositions(tileMap, spawn);

		if (spawnPositions.Count > 0)
		{
			spawn.GlobalPosition = GetValidSpawnPositions(tileMap, spawn).PickRandom();
		}
		else
		{
			spawn.QueueFree();
		}
		
	}

	/// <summary>
	/// Returns an array of valid spawn positions as Vector2s
	/// </summary>
	/// <param name="tileMap">The tilemap to check for valid positions on</param>
	/// <param name="spawn">The spawn to be placed</param>
	/// <returns>Array of Vector2 valid spawn positions</returns>
	private Array<Vector2> GetValidSpawnPositions(TileMapLayer tileMap, Node2D spawn)
	{
		Array<Vector2I> tiles = tileMap.GetUsedCells();
		Array<Vector2> validPositions = new Array<Vector2>();
		ShapeCast2D spawnBuffer = spawn.GetNode<ShapeCast2D>("SpawnBuffer");

		for (int i=0; i < tiles.Count; i++)
		{
			//Gives a position slightly above the tile
			spawnBuffer.Position = tileMap.MapToLocal(new Vector2I(tiles[i].X, tiles[i].Y-1)) + new Vector2(0, 2); 

			// Force the spawnBuffer to update
			spawnBuffer.ForceUpdateTransform();
			spawnBuffer.ForceShapecastUpdate();

			if (!spawnBuffer.IsColliding())
			{
				validPositions.Add(spawnBuffer.Position);
			}
			
		}

		spawnBuffer.Position = Vector2.Zero; // Reset the spawnBuffer position

		return validPositions;
	}
}
