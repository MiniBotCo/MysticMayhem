using System.Collections.Generic;
using Godot;

public partial class Spawner : Node2D
{
	private List<ISpawnable> spawnables = new List<ISpawnable>();

	/// <summary>
	/// Adds a spawnable to the list of things to spawn
	/// </summary>
	/// <param name="spawnable">The spawnable to add</param>
	public void AddToSpawnList(ISpawnable spawnable)
	{
		spawnables.Add(spawnable);
	}

	/// <summary>
	/// Spawns eveything on the spawnables list
	/// </summary>
	/// <param name="tileMapLayers">The tile map layers to spawn on</param>
	/// <param name="unusedTiles">The unused tiles</param>
	public void Spawn(List<TileMapLayer> tileMapLayers, List<Vector2I> unusedTiles)
	{
		foreach(ISpawnable spawn in spawnables)
		{
			if(spawn.spawner != null)
			{
				spawn.spawner.SpawnEntity(tileMapLayers, unusedTiles);
			}
			else
			{
				GD.Print("Spawnable:  " + spawn + " does not have a valid spawner node assigned");
			}
		}
	}
}