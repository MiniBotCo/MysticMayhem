using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;

public partial class LevelGenerator : Node2D
{
    [ExportGroup("Level Properties")]
    [Export]
    public Vector2 LevelSize { get; set; } = new Vector2(20, 20);
    [Export]
    public int PlatformCount { get; set; } = 20;
    [Export]
    public int PlatformGaps { get; set; } = 6;

    private TileMapLayer _frameTileMap;
    private TileMapLayer _platformTileMap;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        GD.Randomize();
        
        _frameTileMap = GetNode<TileMapLayer>("FrameTileMap");
        _platformTileMap = GetNode<TileMapLayer>("PlatformTileMap");

        GenerateFrame();
        PlacePlatforms();
    }

    /// <summary>
    /// Draws the frame with the size given in LevelSize
    /// </summary>
    private void GenerateFrame()
    {
        for (int y = 0; y <= LevelSize.Y; y++)
        {
            if (y == 0 || y == LevelSize.Y) //Top and bottom
            {
                for (int x = 0; x <= LevelSize.X; x++)
                {
                    _frameTileMap.SetCell(new Vector2I(x, y), 0, new Vector2I(2, 2));
                }
            }
            else // Sides
            {
                _frameTileMap.SetCell(new Vector2I(0, y), 0, new Vector2I(2, 2));
                _frameTileMap.SetCell(new Vector2I((int)LevelSize.X, y), 0, new Vector2I(2, 2));
            }
        }
    }

    /// <summary>
    /// Creates the random platforms and fills the level with them
    /// </summary>
    private void PlacePlatforms()
    {
        int row = (int)LevelSize.Y;
        int platforms = 0;

        while(platforms < PlatformCount)
        {
            BasicPlatform platform = new BasicPlatform();

            List<Vector2I> validPositions = GetValidPlatformPositions(platform.GetBuffer(), row);

            if (validPositions.Count < PlatformGaps) // Use valid platform positions until there are only PlatformGaps amount left
            {
                if (row > platform.GetBuffer().Size.Y * 2) // If the current row isn't within double the platform buffer's distance from the top
                {
                    row--; // Go to the next row
                }
                else
                {
                    return; // Exit the function
                }
            }
            else // Create the platform
            {
                platform.SetPosition(validPositions[(int)(GD.Randi() % validPositions.Count)]); // Get a random valid position from the list
                platform.DrawPlaftform(_platformTileMap); // Draw the platform
                platforms++; // Increment the platform counter
            }
        }
    }

    /// <summary>
    /// Checks if a platform colides with another platform. Should be run before the platform is drawn.
    /// </summary>
    /// <param name="platform">The platform to check the validity of</param>
    /// <returns></returns>
    private bool IsValidBufferPosition(Rect2I buffer)
    {
        for (int x = 0; x < buffer.Size.X; x++)
        {
            for (int y = 0; y < buffer.Size.Y; y++)
            {
                TileData tile = _platformTileMap.GetCellTileData(new Vector2I(x, y) + buffer.Position); // Get the tile information at x, y

                if(tile != null) // If the tile exists
                {
                    return false;
                }
            }
        }
        return true; // If no tile exists
    }

    /// <summary>
    /// Returns a list of valid platfrom positions.
    /// </summary>
    /// <param name="buffer"></param> The platform buffer to be checking with
    /// <param name="row"></param> The row to be cecking on
    /// <returns></returns>
    private List<Vector2I> GetValidPlatformPositions(Rect2I buffer, int row)
    {
        List<Vector2I> validPositions = new List<Vector2I>();

        // Loops through all the x position along the row and check if they are valid. If so they are added to the list
        for (int x = 1; x < LevelSize.X  - (buffer.Size.X - 1); x++)
        {
            Vector2I position = new Vector2I(x, row - buffer.Size.Y);

            buffer.Position = position;
            if(IsValidBufferPosition(buffer))
            {
                validPositions.Add(position);
            }
        }

        return validPositions;
    }

}
