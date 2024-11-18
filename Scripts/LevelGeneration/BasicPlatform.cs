using Godot;

public class BasicPlatform
{
	private Vector2I _maxSize = new Vector2I(5, 1);
	private Vector2I _minSize = new Vector2I(3, 1);

	// NOT the size of the total buffer, this is how much the buffer extends outside of the platform
	private Vector2I _bufferSize = new Vector2I(3, 7); 

	private Rect2I _buffer;
	private Rect2I _tiles;

	public BasicPlatform()
	{
		Vector2I size = new Vector2I(GD.RandRange(_minSize.X, _maxSize.X), (int)GD.Randi() % _maxSize.Y + 1); //Create the random sized platform
		Vector2I bufferSize = size + _bufferSize; // Set the buffer to its proper size
		_buffer = new Rect2I(new Vector2I(0, 0), bufferSize);
		_tiles = new Rect2I(new Vector2I(0, 0), size);
	}

	/// <summary>
	/// Returns the platform buffer
	/// </summary>
	/// <returns>The platform buffer</returns>
	public Rect2I GetBuffer() 
	{
		return _buffer;
	}

	/// <summary>
	/// Gets the platform tiles size
	/// </summary>
	/// <returns>Tiles size</returns>
	public Rect2I GetTiles()
	{
		return _tiles;
	}

	/// <summary>
	/// Sets the platform position to the position provided.
	/// </summary>
	/// <param name="pos"></param> Position for the platform to be set to
	public void SetPosition(Vector2I pos)
	{
		_buffer.Position = pos;
	}

	/// <summary>
	/// Draws the platform on the TileMapLayer passed to it.
	/// </summary>
	/// <param name="tileMap"></param> TileMapLayer to draw on
	public void DrawPlaftform(TileMapLayer tileMap)
	{
		_tiles.Position = _buffer.GetCenter();

		// Runs differently from center depending on if the size is even or odd
		if (_tiles.Size.X % 2 == 1)
		{
			for (int x = -_tiles.Size.X/2; x <= _tiles.Size.X/2; x++)
			{
				for (int y = 0; y < _tiles.Size.Y; y++)
				{
					tileMap.SetCell(_tiles.Position + new Vector2I(x, y), 2, new Vector2I(0, 0));
				}
			}
		}
		else
		{
			for (int x = -_tiles.Size.X/2; x < _tiles.Size.X/2; x++)
			{
				for (int y = 0; y < _tiles.Size.Y; y++)
				{
					tileMap.SetCell(_tiles.Position + new Vector2I(x, y), 2, new Vector2I(0,0));
				}
			}
		}
	}
}