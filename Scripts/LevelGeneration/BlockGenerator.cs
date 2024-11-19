using Godot;
using System;

public class Block
{
    int Smoothness = 3;
    private Rect2I block;

    /// <summary>
    /// Creates a random block given a position and a maximum and minumum size, returns the block size
    /// </summary>
    /// <param name="pos">The block position</param>
    /// <param name="min">The minimum size for the block</param>
    /// <param name="max">The maximum size for the block</param>
    /// <returns>The size of the block</returns>
    public Vector2I GenerateBlock(Vector2I pos, Vector2I min, Vector2I max)
    {
        Vector2I size = new Vector2I(GD.RandRange(min.X, max.X), GD.RandRange(min.Y, max.Y));

        for(int i = 0; i < Smoothness; i++) {
            size = new Vector2I(Math.Min(size.X, GD.RandRange(min.X, max.X)), Math.Min(size.Y, GD.RandRange(min.Y, max.Y)));
        }

        block = new Rect2I(pos, size);

        return size;
    }

    /// <summary>
    /// Draws a block within the levelSize on the given tilemap layer
    /// </summary>
    /// <param name="blockTileMap">The tilemap to draw to</param>
    /// <param name="levelSize">The bounds to draw within</param>
    public void DrawBlock(TileMapLayer blockTileMap, Vector2I levelSize, Vector2I tile, bool erase = false)
    {
        for (int x = 0; x <= block.Size.X; x++)
        {
            for (int y = 0; y <= block.Size.Y; y++)
            {
                Vector2I position = new Vector2I(block.Position.X + x - block.Size.X/2, block.Position.Y + y - block.Size.Y/2);
                if (position.X < levelSize.X && position.Y < levelSize.Y && position.X > 0 && position.Y > 0)
                {
                    if (!erase) blockTileMap.SetCell(position, 0, tile);
                    else blockTileMap.EraseCell(position);
                }
            }
        }
    }
}