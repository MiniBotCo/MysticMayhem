using Godot;
using Godot.Collections;
using System;

public partial class DoorSpawnerAlgorithm : SpawnerAlgorithm
{
    /// <summary>
    /// Checks if an empty cell has a cell under it and an empty cell above
    /// </summary>
    /// <param name="tileMaps">The TileMapLayers to check for cells on</param>
    /// <param name="pos">The position being checked</param>
    /// <returns></returns>
	public override Array<Vector2I> CheckSpawnValidity(Array<TileMapLayer> tileMaps, Vector2I pos)
    {
        Array<Vector2I> usedTiles = new Array<Vector2I>();

        for(int i = 0; i < tileMaps.Count; i++)
        {
            if(tileMaps[i].GetCellTileData(pos - new Vector2I(0, -1)) != null && tileMaps[i].GetCellTileData(pos - new Vector2I(0, 1)) == null)
            {
                usedTiles.Add(pos);
				usedTiles.Add(pos - new Vector2I(0, 1));
            }
        }

        return usedTiles;
    }
}
