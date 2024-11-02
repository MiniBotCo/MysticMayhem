using Godot;
using Godot.Collections;

public partial class Spawner : Node2D
{
	[Export]
	public int spawnCount { get; set; } = 5;
	[Export]
	public Vector2I levelSize { get; set; } = new Vector2I(60, 40);

	[Export]
	public int yOffset{ get; set; } = 0;

	/// <summary>
	/// Spawns the spawnable nodes from and array of spawnable nodes on an unused tile that meets the spawning requirements for that node
	/// </summary>
	/// <param name="spawnables">An array of spawnable nodes</param>
	/// <param name="unusedTiles">The tiles that are not used</param>
	/// <param name="tileMaps">An array of used TileMapLayers</param>
	/// <returns>The uptades unusedTiles array</returns>
	public Array<Vector2I> Spawn(Array<Node2D> spawnables, Array<Vector2I> unusedTiles, Array<TileMapLayer> tileMaps)
	{
		Array<Vector2I> usedTiles = new Array<Vector2I>();

		for(int i = 0; i < spawnables.Count; i++)
		{
			SpawnAlgorithm spawnAlgorithm;

			if (spawnables[i].IsInGroup("Spawnable") && (spawnAlgorithm = spawnables[i].GetNodeOrNull<SpawnAlgorithm>("SpawnAlgorithm")) != null)
			{
				Vector2I spawnTile = GetValidTile(spawnAlgorithm, unusedTiles, tileMaps);
				spawnables[i].Position = tileMaps[0].MapToLocal(spawnTile);

				Array<Vector2I> spawnTiles = spawnAlgorithm.spawnerAlgorithm.CheckSpawnValidity(tileMaps, spawnTile);

				for(int j = 0; j < spawnTiles.Count; j++)
				{
					usedTiles.Add(spawnTiles[j]);
				}

			}
		}

		for (int i = 0; i < usedTiles.Count; i++)
		{
			unusedTiles.Remove(usedTiles[i]);
		}

		return unusedTiles;
	}

	/// <summary>
	/// Returns a valid spawning tile to be used
	/// </summary>
	/// <param name="spawnableNode">The node to be spawned</param>
	/// <param name="unusedTiles">An array of unused tiles</param>
	/// <param name="tileMaps">An array of tilemaps</param>
	/// <returns>A random cell</returns>
	private Vector2I GetValidTile(SpawnAlgorithm spawnableNode,  Array<Vector2I> unusedTiles, Array<TileMapLayer> tileMaps)
	{
		Array<Vector2I> validTiles = new Array<Vector2I>();

		for(int i = 0; i < unusedTiles.Count; i++)
		{
			Array<Vector2I> usableTiles = spawnableNode.spawnerAlgorithm.CheckSpawnValidity(tileMaps, unusedTiles[i]);
			if (usableTiles.Count != 0)
			{
				validTiles.Add(unusedTiles[i]);
			}
		}

		return validTiles.PickRandom();
	}
}