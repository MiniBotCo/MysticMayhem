using Godot;
using Godot.Collections;

public partial class PlatformGenerator : Node2D
{
    [Export]
    public Vector2I PlatformBounds { get; set; } = new Vector2I(40, 60);
	[Export]
    public int PlatformCount { get; set; } = 20;
    [Export]
    public int PlatformGaps { get; set; } = 6;

    private TileMapLayer _platformTileMap;
	private Array<TileMapLayer> _checkLayers = new Array<TileMapLayer>();

    public override void _Ready()
    {
        _platformTileMap = GetNode<TileMapLayer>("PlatformTileMapLayer");
		AddCheckLayer(_platformTileMap);
    }

    public TileMapLayer GetTileMapLayer()
    {
        return _platformTileMap;
    }

    /// <summary>
    /// Set the bounds in which platforms can spawn
    /// </summary>
    /// <param name="bounds">The bounds</param>
    public void SetBounds(Vector2I bounds)
    {
        PlatformBounds = bounds;
    }

    /// <summary>
    /// Adds a layer to check for platform collisions with
    /// </summary>
    /// <param name="layer">The layer to check</param>
	public void AddCheckLayer(TileMapLayer layer)
	{
		_checkLayers.Add(layer);
	}

    /// <summary>
    /// Adds an array of layers to check for platform collisions with
    /// </summary>
    /// <param name="layer">The array of layers</param>
	public void AddCheckLayer(Array<TileMapLayer> layer)
	{
		_checkLayers.AddRange(layer);
	}

    /// <summary>
    /// Creates the random platforms and fills the level with them
    /// </summary>
    public void GeneratePlatforms()
    {
        int row = PlatformBounds.Y;
        int platforms = 0;

        while(platforms < PlatformCount)
        {
            BasicPlatform platform = new BasicPlatform();

           Array<Vector2I> validPositions = GetValidPlatformPositions(platform.GetBuffer(), row);

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
                platform.SetPosition(validPositions.PickRandom()); // Get a random valid position from the list
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
				for (int layer = 0; layer < _checkLayers.Count; layer++)
				{
					TileData tile = _checkLayers[layer].GetCellTileData(new Vector2I(x, y) + buffer.Position); // Get the tile information at x, y

                	if(tile != null) // If the tile exists
                	{
                   		return false;
                	}
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
    private Array<Vector2I> GetValidPlatformPositions(Rect2I buffer, int row)
    {
        Array<Vector2I> validPositions = new Array<Vector2I>();

        // Loops through all the x position along the row and check if they are valid. If so they are added to the list
        for (int x = 1; x < PlatformBounds.X  - (buffer.Size.X - 1); x++)
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
