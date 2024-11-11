using Godot;

public partial class FrameGenerator : Node2D
{
    [Signal]
    public delegate void GenerationCompleteEventHandler();

    [ExportGroup("Frame Properties")]
    [Export]
    public Vector2I FrameSize { get; set; } = new Vector2I(40, 60);
    [Export]
    public Vector2I FrameBlockMinSize { get; set; } = new Vector2I(5, 5);
    [Export]
    public Vector2I FrameBlockMaxSize { get; set; } = new Vector2I(15, 10);

    private TileMapLayer _frameTileMap;


    public override void _Ready()
    {
        _frameTileMap = GetNode<TileMapLayer>("FrameTileMapLayer");
    }

    /// <summary>
    /// Gets the TileMapLayer
    /// </summary>
    /// <returns>TileMapLayer</returns>
    public TileMapLayer GetTileMapLayer()
    {
        return _frameTileMap;
    }

    /// <summary>
    /// Sets the frame size
    /// </summary>
    /// <param name="size">Size to eb set to</param>
    public void SetFrameSize(Vector2I size)
    {
        FrameSize = size;
    }

    /// <summary>
    /// Draws the frame with the size given in FrameSize
    /// </summary>
    public void GenerateFrame()
    {
        for (int y = 0; y <= FrameSize.Y; y++)
        {
            if (y == 0 || y == FrameSize.Y) //Top and bottom
            {
                for (int x = 0; x <= FrameSize.X; x++)
                {
                    _frameTileMap.SetCell(new Vector2I(x, y), 0, new Vector2I(4, 0));
                }
            }
            else // Sides
            {
                _frameTileMap.SetCell(new Vector2I(0, y), 0, new Vector2I(4, 0));
                _frameTileMap.SetCell(new Vector2I(FrameSize.X, y), 0, new Vector2I(4, 0));
            }
        }
        GenerateSides();
    }

    /// <summary>
    /// Generates the sides of the level out of blocks
    /// </summary>
    private void GenerateSides()
    {
        int n = 0;
        while (n <= FrameSize.Y)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(0, n), FrameBlockMinSize, FrameBlockMaxSize).Y/2;
            b.DrawBlock(_frameTileMap, FrameSize);
        }
        n = 0;
        while (n <= FrameSize.Y)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(FrameSize.X, n), FrameBlockMinSize, FrameBlockMaxSize).Y/2;
            b.DrawBlock(_frameTileMap, FrameSize);
        }
        n = 0;
        while (n <= FrameSize.X)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(n, 0), FrameBlockMinSize, FrameBlockMaxSize).X/2;
            b.DrawBlock(_frameTileMap, FrameSize);
        }
        n = 0;
        while (n <= FrameSize.X)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(n, FrameSize.Y), FrameBlockMinSize, FrameBlockMaxSize).X/2;
            b.DrawBlock(_frameTileMap, FrameSize);
        }
    }
}
