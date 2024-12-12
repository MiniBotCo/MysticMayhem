using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class LevelGenerator : Node2D
{
    [ExportGroup("Level Properties")]
    [Export]
    public Vector2I LevelSize { get; set; } = new Vector2I(60, 40);

    public Door Entrance { get; set; }
    public Door Exit { get; set; }
    public Player Player { get; set;}

    private FrameGenerator _frameGenerator;
    private PlatformGenerator _platformGenerator;
    private Spawner _spawner;
    private Chest _chest;
    private EnemyGenerator _enemyGenerator;

    private List<TileMapLayer> _tileMapLayers;
    private List<Vector2I> _unusedTiles;

    
    // For debug purposes, remove for rinal release
    private TileMapLayer _debugTileMapLayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Entrance = GetNode<Door>("Entrance");
        Exit = GetNode<Door>("Exit");
        Player = GetNode<Player>("Player");
        // Initializing the child nodes
        _frameGenerator = GetNode<FrameGenerator>("FrameGenerator");
        _platformGenerator = GetNode<PlatformGenerator>("PlatformGenerator");
        _enemyGenerator = GetNode<EnemyGenerator>("EnemyGenerator");
        _spawner = GetNode<Spawner>("Spawner");

        // For debug purposes, remove for rinal release
        _debugTileMapLayer = GetNode<TileMapLayer>("DebugTileMapLayer");

        _tileMapLayers = new List<TileMapLayer>();
        _unusedTiles = new List<Vector2I>();

        _chest = GetNode<Chest>("Chest");

        _enemyGenerator.EnemiesDefeated += LevelComplete;
    }

    /// <summary>
    /// Generates the level
    /// </summary>
    public async void GenerateLevel(int level)
    {
        // Randomizes the scene
        GD.Randomize();

        // Creates a frame
        _frameGenerator.SetFrameSize(LevelSize);
        _frameGenerator.GenerateFrame();

        // Adds the frame TileMapLayer to the list of TileMapLayers
        _tileMapLayers.Add(_frameGenerator.GetTileMapLayer());

        // TODO Make platform generator use the spawner node
        _platformGenerator.AddCheckLayer(_frameGenerator.GetTileMapLayer());

        // Genertate the platforms
        _platformGenerator.SetBounds(LevelSize);
        _platformGenerator.GeneratePlatforms();

        // Adds the platfrom TileMapLayer 
        _tileMapLayers.Add(_platformGenerator.GetTileMapLayer());

        // Gets all of the unused tiles
        _unusedTiles = GetUnusedTiles(_tileMapLayers);

        // Spawns the entrance and exit doors
        _spawner.AddToSpawnList(Entrance);
        _spawner.AddToSpawnList(Exit);
        _spawner.AddToSpawnList(_chest);

        _enemyGenerator.GenerateEnemies(level, _spawner);
        await ToSignal(_enemyGenerator, EnemyGenerator.SignalName.EnemiesGenerated);

        // Spawns the spawnable list
        _spawner.Spawn(_tileMapLayers, _unusedTiles);

        // Draw the debug tiles, to be removed in full release
        for (int i = 0; i < _unusedTiles.Count; i++)
        {
            _debugTileMapLayer.SetCell(_unusedTiles[i], 1, new Vector2I(0, 0));
        }

        // Spawn the player
        SpawnPlayer();
    }

    /// <summary>
    /// Spawns the player at the entrance door
    /// </summary>
    private void SpawnPlayer()
    {
        if (Entrance != null)
        {
            Player.Position = Entrance.Position + new Vector2(0, -32);

            Vector2I levelPixels = (Vector2I)(_frameGenerator.GetTileMapLayer().MapToLocal(LevelSize) + new Vector2(16, -16));
            Player.GetNode<Camera2D>("Camera2D").LimitBottom = levelPixels.Y;
            Player.GetNode<Camera2D>("Camera2D").LimitTop = 32;
            Player.GetNode<Camera2D>("Camera2D").LimitRight = levelPixels.X;
            Player.GetNode<Camera2D>("Camera2D").LimitLeft = 0;
        }
    }



    /// <summary>
    /// Returns an array of Vector2I containing all unused tiles within the level bounds
    /// </summary>
    /// <param name="tileMaps">A list of used TileMapLayers</param>
    /// <returns></returns>
    private List<Vector2I> GetUnusedTiles(List<TileMapLayer> tileMaps)
    {
        // Array to hold the unused tiles
        List<Vector2I> unusedTiles = new List<Vector2I>();

        // Gets all tiles withing level bounds
        for(int x = _frameGenerator.FrameBlockMaxSize.X; x <= LevelSize.X - _frameGenerator.FrameBlockMaxSize.X; x++)
        {
            for(int y = 0; y <= LevelSize.Y; y++)
            {
                unusedTiles.Add(new Vector2I(x, y));
            }
        }

        // Loops through all used TileMapLayers
        for (int i = 0; i < tileMaps.Count; i++)
        {
            // Gets their used cells
            Array<Vector2I> tileMapTiles = tileMaps[i].GetUsedCells();

            // Removes their used cells from the list of unused cells
            for (int j = 0; j < tileMapTiles.Count; j++)
            {
                unusedTiles.Remove(tileMapTiles[j]);
                _debugTileMapLayer.SetCell(tileMapTiles[j], 0, new Vector2I(0, 0)); // For debug purposes, remove for final release
            }
        }

        Shuffle<Vector2I>.ShuffleList(unusedTiles);

        return unusedTiles;
    }

    private void LevelComplete()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = GD.Load<AudioStreamWav>("res://Assets/Sfx/levelComplete.wav");
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        _chest.Unlock();
        Exit.Unlock();
    }
}


