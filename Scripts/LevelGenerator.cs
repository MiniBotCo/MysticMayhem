using Godot;
using System;
using System.Collections.Generic;
public partial class LevelGenerator : Node2D
{
    [ExportGroup("Level Properties")]
    [Export]
    public Vector2I LevelSize { get; set; } = new Vector2I(60, 40);
    [Export]
    public int PlatformCount { get; set; } = 20;
    [Export]
    public int PlatformGaps { get; set; } = 6;
    [Export]
    public int Smoothness { get; set; } = 3;
    [Export]
    public int Blobs { get; set; } = 2;
    [Export]
    public Vector2I FrameBlockMinSize { get; set; } = new Vector2I(5, 5);
    [Export]
    public Vector2I FrameBlockMaxSize { get; set; } = new Vector2I(15, 10);
    [ExportGroup("Blob Properties")]
    [Export]
    public Vector2I BlobMaxSize { get; set; } = new Vector2I(7, 7);
    [Export]
    public Vector2I BlobMinSize { get; set; } = new Vector2I(5, 5);

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
        GenerateSides();
        for (int i=0; i < Blobs; i++) GenerateBlob();
    }

    private void GenerateSides()
    {
        int n = 0;
        while (n <= LevelSize.Y)
        {
            n += GenerateBlock(new Vector2I(0, n), FrameBlockMinSize, FrameBlockMaxSize).Y/2;
        }
        n = 0;
        while (n <= LevelSize.Y)
        {
            n += GenerateBlock(new Vector2I(LevelSize.X, n), FrameBlockMinSize, FrameBlockMaxSize).Y/2;
        }
        n = 0;
        while (n <= LevelSize.X)
        {
            n += GenerateBlock(new Vector2I(n, 0), FrameBlockMinSize, FrameBlockMaxSize).X/2;
        }
        n = 0;
        while (n <= LevelSize.X)
        {
            n += GenerateBlock(new Vector2I(n, LevelSize.Y), FrameBlockMinSize, FrameBlockMaxSize).X/2;
        }
    }

    private void GenerateBlob()
    {
        Vector2I position = LevelSize/2 + new Vector2I(GD.RandRange(-LevelSize.X/3, LevelSize.X/3), GD.RandRange(-LevelSize.Y/3, LevelSize.Y/3));
        for(int i = 0; i < 5; i++)
        {
            GenerateBlock(new Vector2I(position.X + GD.RandRange(-3, 3), position.Y + GD.RandRange(-3, 3)), BlobMinSize, BlobMaxSize);
        }
    }

    private Vector2I GenerateBlock(Vector2I pos, Vector2I min, Vector2I max)
    {
        Vector2I size = new Vector2I(GD.RandRange(min.X, max.X), GD.RandRange(min.Y, max.Y));

        for(int i = 0; i < Smoothness; i++) {
            size = new Vector2I(Math.Min(size.X, GD.RandRange(min.X, max.X)), Math.Min(size.Y, GD.RandRange(min.Y, max.Y)));
        }

        Rect2I block = new Rect2I(pos, size);

        DrawBlock(block);

        return size;
    }

    private void DrawBlock(Rect2I block)
    {
        for (int x = 0; x <= block.Size.X; x++)
        {
            for (int y = 0; y <= block.Size.Y; y++)
            {
                Vector2I position = new Vector2I(block.Position.X + x - block.Size.X/2, block.Position.Y + y - block.Size.Y/2);
                if (position.X < LevelSize.X && position.Y < LevelSize.Y && position.X > 0 && position.Y > 0)
                {
                    _frameTileMap.SetCell(position, 0, new Vector2I(2, 2));
                }
            }
        }
    }
    /// <summary>
    /// Creates the random platforms and fills the level with them
    /// </summary>
    private void PlacePlatforms()
    {
        int row = LevelSize.Y;
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

                tile = _frameTileMap.GetCellTileData(new Vector2I(x, y) + buffer.Position); // Get the tile information at x, y

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