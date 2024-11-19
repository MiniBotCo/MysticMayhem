using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class LevelGenerator : Node2D
{
    [ExportGroup("Level Properties")]
    [Export]
    public Vector2I LevelSize { get; set; } = new Vector2I(60, 40);

    [Export]
    public Door Entrance { get; set; } = null;
    [Export]
    public Door Exit { get; set; } = null;

    [Export]
    public PackedScene PlayerScene { get; set;} = null;

    private FrameGenerator _frameGenerator;
    private PlatformGenerator _platformGenerator;
    private Spawner _spawner;
    private Chest _chest;


    private bool _inDoor = false;
    private List<TileMapLayer> _tileMapLayers;
    private List<Vector2I> _unusedTiles;

    
    // For debug purposes, remove for rinal release
    private TileMapLayer _debugTileMapLayer;
    private Enemy _enemy;
    private AnimationPlayer _animationPlayer;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _enemy = GetNode<Enemy>("Enemy");
        // Initializing the child nodes
        _frameGenerator = GetNode<FrameGenerator>("FrameGenerator");
        _platformGenerator = GetNode<PlatformGenerator>("PlatformGenerator");
        _spawner = GetNode<Spawner>("Spawner");

        // For debug purposes, remove for rinal release
        _debugTileMapLayer = GetNode<TileMapLayer>("DebugTileMapLayer");

        _tileMapLayers = new List<TileMapLayer>();
        _unusedTiles = new List<Vector2I>();

        _chest = GetNode<Chest>("Chest");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        base._Ready();

        // Creates the level
        GenerateLevel();
    }

    /// <summary>
    /// Generates the level
    /// </summary>
    public void GenerateLevel()
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
        _spawner.AddToSpawnList(_enemy);

        // Signals for the exit door, used to exit the level
        Exit.BodyEntered += OnExitBodyEntered;
        Exit.BodyExited += OnExitBodyExited;

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
    private async void SpawnPlayer()
    {
        if (Entrance != null)
        {
            Node2D player = PlayerScene.Instantiate<Node2D>();
            CallDeferred(Node.MethodName.AddChild, player);
            await ToSignal(player, Node2D.SignalName.Ready);
            player.Position = Entrance.Position + new Vector2(0, -32);

            Vector2I levelPixels = (Vector2I)(_frameGenerator.GetTileMapLayer().MapToLocal(LevelSize) + new Vector2(16, -16));
            player.GetNode<Camera2D>("Camera2D").LimitBottom = levelPixels.Y;
            player.GetNode<Camera2D>("Camera2D").LimitTop = 32;
            player.GetNode<Camera2D>("Camera2D").LimitRight = levelPixels.X;
            player.GetNode<Camera2D>("Camera2D").LimitLeft = 0;
        }
    }

    /// <summary>
    /// Handles input, only used here for the exit door
    /// </summary>
    /// <param name="event">The input event</param>
    public override async void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("EnterDoor") && _inDoor)
        {
            _animationPlayer.Play("ChangeScene");
            await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
            // TODO make this switch to a transition scene
            GetTree().ChangeSceneToFile("res://Scenes/level_generator.tscn");
        }
        
        base._Input(@event);
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

    /// <summary>
    /// Called when the Exit door has a body enter
    /// </summary>
    /// <param name="body">The body entered</param>
    private void OnExitBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player")) _inDoor = true;
    }

    /// <summary>
    /// Called when the Exit door has a body exit
    /// </summary>
    /// <param name="body">The body exited</param>
    private void OnExitBodyExited(Node2D body)
    {
        if (body.IsInGroup("Player")) _inDoor = false;
    }
}


