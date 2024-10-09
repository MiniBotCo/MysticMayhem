using Godot;
using System;

public partial class FrameGenerator : Node2D
{
    [ExportGroup("Frame Properties")]
    [Export]
    public Vector2I FrameBlockMinSize { get; set; } = new Vector2I(5, 5);
    [Export]
    public Vector2I FrameBlockMaxSize { get; set; } = new Vector2I(15, 10);

    [ExportGroup("Blob Properties")]
    [Export]
    public int Blobs { get; set; } = 2;
    [Export]
    public Vector2I BlobMaxSize { get; set; } = new Vector2I(7, 7);
    [Export]
    public Vector2I BlobMinSize { get; set; } = new Vector2I(5, 5);

    private TileMapLayer _frameTileMap;

    public override void _Ready()
    {
        _frameTileMap = GetNode<TileMapLayer>("FrameTileMapLayer");
    }

    public TileMapLayer GetTileMapLayer()
    {
        return _frameTileMap;
    }

    /// <summary>
    /// Draws the frame with the size given in LevelSize
    /// </summary>
    public void GenerateFrame(Vector2I levelSize)
    {
        for (int y = 0; y <= levelSize.Y; y++)
        {
            if (y == 0 || y == levelSize.Y) //Top and bottom
            {
                for (int x = 0; x <= levelSize.X; x++)
                {
                    _frameTileMap.SetCell(new Vector2I(x, y), 0, new Vector2I(2, 2));
                }
            }
            else // Sides
            {
                _frameTileMap.SetCell(new Vector2I(0, y), 0, new Vector2I(2, 2));
                _frameTileMap.SetCell(new Vector2I(levelSize.X, y), 0, new Vector2I(2, 2));
            }
        }
        GenerateSides(levelSize);
        for (int i = 0; i < Blobs; i++) GenerateBlob(levelSize);
    }

    /// <summary>
    /// Generates the sides of the level out of blocks
    /// </summary>
    private void GenerateSides(Vector2I levelSize)
    {
        int n = 0;
        while (n <= levelSize.Y)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(0, n), FrameBlockMinSize, FrameBlockMaxSize).Y/2;
            b.DrawBlock(_frameTileMap, levelSize);
        }
        n = 0;
        while (n <= levelSize.Y)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(levelSize.X, n), FrameBlockMinSize, FrameBlockMaxSize).Y/2;
            b.DrawBlock(_frameTileMap, levelSize);
        }
        n = 0;
        while (n <= levelSize.X)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(n, 0), FrameBlockMinSize, FrameBlockMaxSize).X/2;
            b.DrawBlock(_frameTileMap, levelSize);
        }
        n = 0;
        while (n <= levelSize.X)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(n, levelSize.Y), FrameBlockMinSize, FrameBlockMaxSize).X/2;
            b.DrawBlock(_frameTileMap, levelSize);
        }
    }

    /// <summary>
    /// Creates a blob out of blocks using the predefined blob size
    /// </summary>
    private void GenerateBlob(Vector2I levelSize)
    {
        Vector2I position = levelSize/2 + new Vector2I(GD.RandRange(-levelSize.X/3, levelSize.X/3), GD.RandRange(-levelSize.Y/3, levelSize.Y/3));
        for(int i = 0; i < 5; i++)
        {
            Block b = new Block();
            b.GenerateBlock(new Vector2I(position.X + GD.RandRange(-3, 3), position.Y + GD.RandRange(-3, 3)), BlobMinSize, BlobMaxSize);
            b.DrawBlock(_frameTileMap, levelSize);
        }
    }
}
