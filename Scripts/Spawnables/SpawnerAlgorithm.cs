using Godot;
using Godot.Collections;

public partial class SpawnerAlgorithm : Node
{
    /// <summary>
    /// Checks if a given cell has a tile below it. Meant to be used as a base for spawning
    /// </summary>
    /// <param name="tileMaps">An array of tilemaps to check on</param>
    /// <param name="pos">The position to check</param>
    /// <returns>The used tiles</returns>
    public virtual Array<Vector2I> CheckSpawnValidity(Array<TileMapLayer> tileMaps, Vector2I pos)
    {
        Array<Vector2I> usedTiles = new Array<Vector2I>();

        for(int i = 0; i < tileMaps.Count; i++)
        {
            if(tileMaps[i].GetCellTileData(pos - new Vector2I(0, -1)) != null)
            {
                usedTiles.Add(pos);
            }
        }

        return usedTiles;
    }
}