using Godot;
using System.Collections.Generic;

public partial class EnemySpawn : Spawn
{
    /// <summary>
    /// Spawns the Door and removes used cells from list of valid ones
    /// </summary>
    /// <param name="tileMapLayers">The TileMapLayers in use</param>
    /// <param name="unusedTiles">The unused tiles</param>
    public override void SpawnEntity(List<TileMapLayer> tileMapLayers, List<Vector2I> unusedTiles)
    {
		List<Vector2I> usedCells = PlaceSpawn(tileMapLayers, unusedTiles);

		foreach (Vector2I cell in usedCells)
		{
			unusedTiles.Remove(cell);
		}
    }

    /// <summary>
    /// Places the Door at a valid position and returns the tiles used
    /// </summary>
    /// <param name="tileMapLayers">The tilemapLayers in use</param>
    /// <param name="unusedTiles">The unused tiles</param>
    /// <returns>The tiles used</returns>
    private List<Vector2I> PlaceSpawn(List<TileMapLayer> tileMapLayers, List<Vector2I> unusedTiles)
	{
		List<Vector2I> usedCells = new List<Vector2I>();

		foreach (Vector2I cell in unusedTiles)
		{
			foreach (TileMapLayer tileMapLayer in tileMapLayers)
			{

				if(tileMapLayer.GetCellTileData(cell) == null && tileMapLayer.GetCellTileData(cell + Vector2I.Up) == null && tileMapLayer.GetCellTileData(cell + Vector2I.Down) != null)
				{
					Node2D parent = GetParentOrNull<Node2D>();

					if (parent != null)
					{
						parent.Position = tileMapLayer.MapToLocal(cell);
						usedCells.Add(cell);
                        usedCells.Add(cell + Vector2I.Up);

						return usedCells;
					}
				}
            }

		}

		return usedCells;
	}

}