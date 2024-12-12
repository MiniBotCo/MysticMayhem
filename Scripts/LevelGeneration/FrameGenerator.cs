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
    [Export]
    public int holes { get; set; } = 14;

    private TileMapLayer _frameTileMap;
    private TileMapLayer _backgroudTileMap;


    public override void _Ready()
    {
        _frameTileMap = GetNode<TileMapLayer>("FrameTileMapLayer");
        _backgroudTileMap = GetNode<TileMapLayer>("BackgroundTileMapLayer");
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
        FrameSize = size + new Vector2I(0, 1);
    }

    /// <summary>
    /// Draws the frame with the size given in FrameSize
    /// </summary>
    public void GenerateFrame()
    {
        GenerateBackground();

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
                _frameTileMap.SetCell(new Vector2I(0, y), 0, new Vector2I(0, 0));
                _frameTileMap.SetCell(new Vector2I(FrameSize.X, y), 0, new Vector2I(0, 0));
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
            b.DrawBlock(_frameTileMap, FrameSize, new Vector2I(0, 0));
        }
        n = 0;
        while (n <= FrameSize.Y)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(FrameSize.X, n), FrameBlockMinSize, FrameBlockMaxSize).Y/2;
            b.DrawBlock(_frameTileMap, FrameSize, new Vector2I(0, 0));
        }
        n = 0;
        while (n <= FrameSize.X)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(n, 0), FrameBlockMinSize, FrameBlockMaxSize).X/2;
            b.DrawBlock(_frameTileMap, FrameSize, new Vector2I(0, 0));
        }
        n = 0;
        while (n <= FrameSize.X)
        {
            Block b = new Block();
            n += b.GenerateBlock(new Vector2I(n, FrameSize.Y), FrameBlockMinSize, FrameBlockMaxSize).X/2;
            b.DrawBlock(_frameTileMap, FrameSize, new Vector2I(0, 0));
        }
    }

    public void GenerateBackground()
    {
        for (int x = 0; x <= FrameSize.X; x++)
        {
            for (int y = 0; y <= FrameSize.Y; y++)
            {
                _backgroudTileMap.SetCell(new Vector2I(x, y), 1, new Vector2I(0, 0));
            }
        }

        for (int i = 0; i < holes; i++)
        {
            Vector2I pos = new Vector2I(GD.RandRange(0, FrameSize.X), GD.RandRange(FrameSize.Y, 0));
            Block b = new Block();

            for (int j = 0; j < 5; j++)
            {
                b.GenerateBlock(pos + new Vector2I(GD.RandRange(-3, 3), GD.RandRange(-3, 3)), new Vector2I(4, 4), new Vector2I(6, 6));
                b.DrawBlock(_backgroudTileMap, FrameSize, Vector2I.Zero, true);
            }
        }
    }
}
