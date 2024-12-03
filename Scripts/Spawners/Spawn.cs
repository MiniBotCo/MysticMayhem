using Godot;
using System;
using System.Collections.Generic;

public partial class Spawn : Node
{

	[Signal] public delegate void EntitySpawnedEventHandler();

	/// <summary>
	/// Spawns an entity
	/// </summary>
	/// <param name="tileMapLayers">The TileMapLayers to spwan on</param>
	/// <param name="unusedTiles"></param>
    public virtual void SpawnEntity(List<TileMapLayer> tileMapLayers, List<Vector2I> unusedTiles)
    {
		
		List<Vector2I> usedCells = PlaceSpawn(tileMapLayers, unusedTiles);

		foreach (Vector2I cell in usedCells)
		{
			unusedTiles.Remove(cell);
		}

    }

	private List<Vector2I> PlaceSpawn(List<TileMapLayer> tileMapLayers, List<Vector2I> unusedTiles)
	{
		List<Vector2I> usedCells = new List<Vector2I>();

		foreach (TileMapLayer tileMapLayer in tileMapLayers)
		{
			foreach (Vector2I cell in unusedTiles)
			{
				if(tileMapLayer.GetCellTileData(cell) == null && tileMapLayer.GetCellTileData(cell + Vector2I.Down) != null)
				{
					Node2D parent = GetParentOrNull<Node2D>();

					if (parent != null)
					{
						parent.Position = tileMapLayer.MapToLocal(cell);
						usedCells.Add(cell);
						return usedCells;
					}
				}
			}
		}

		return usedCells;
	}
}
