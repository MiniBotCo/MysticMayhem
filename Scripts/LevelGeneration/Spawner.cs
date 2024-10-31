using Godot;
using Godot.Collections;

public partial class Spawner : Node2D
{
	[Export]
	public int spawnCount { get; set; } = 5;
	[Export]
	public Vector2I levelSize { get; set; } = new Vector2I(60, 40);

	[Export]
	public PackedScene spawnScene {get; set; } = GD.Load<PackedScene>("res://Scenes/Characters/Character.tscn");

	[Export]
	public int yOffset{ get; set; } = 0;

	/// <summary>
	/// Spawns the spawnable on the tilemap passed. Currently intended only for use with the platform tilemap
	/// </summary>
	/// <param name="tileMap">The tilemap to spawn spawnables on</param>
    public async void Spawn(TileMapLayer tileMap)
	{
		for(int i = 0; i < spawnCount; i++)
		{
			Node2D spawn = spawnScene.Instantiate<Node2D>();
			if (spawn.IsInGroup("Spawnable"))
			{
				GetTree().Root.CallDeferred(Node.MethodName.AddChild, spawn);
				await ToSignal(spawn, Node.SignalName.Ready);
				PlaceSpawn(tileMap, spawn);
			}
		}
	}

	public void Spawn(Array<Node2D> spawnables, TileMapLayer tileMap)
	{
		for(int i = 0; i < spawnables.Count; i++)
		{

			if (spawnables[i].IsInGroup("Spawnable"))
			{
				PlaceSpawn(tileMap, spawnables[i]);
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
			spawnBuffer.Position = tileMap.MapToLocal(new Vector2I(tiles[i].X, tiles[i].Y-1)); 

			// Force the spawnBuffer to update
			spawnBuffer.ForceUpdateTransform();
			spawnBuffer.ForceShapecastUpdate();

			if (!spawnBuffer.IsColliding())
			{
				validPositions.Add(spawnBuffer.Position + new Vector2(0, yOffset));
			}
			
		}

		spawnBuffer.Position = Vector2.Zero; // Reset the spawnBuffer position

		return validPositions;
	}
}
