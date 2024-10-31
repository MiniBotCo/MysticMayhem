using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Collections;

public partial class LevelGenerator : Node2D
{
    [ExportGroup("Level Properties")]
    [Export]
    public Vector2I LevelSize { get; set; } = new Vector2I(60, 40);

    [Export]
    public Area2D Entrance { get; set; } = null;
    [Export]
    public Area2D Exit { get; set; } = null;

    [Export]
    public PackedScene PlayerScene { get; set;} = null;

    private FrameGenerator _frameGenerator;
    private PlatformGenerator _platformGenerator;
    private Spawner _enemySpawner;

    private Spawner _doorSpawner;

    private bool _inDoor = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _frameGenerator = GetNode<FrameGenerator>("FrameGenerator");
        _platformGenerator = GetNode<PlatformGenerator>("PlatformGenerator");
        _enemySpawner = GetNode<Spawner>("EnemySpawner");
        _doorSpawner = GetNode<Spawner>("DoorSpawner");

        base._Ready();

        GenerateLevel();
    }

    public void GenerateLevel()
    {

        GD.Randomize();

        _frameGenerator.SetFrameSize(LevelSize);
        _frameGenerator.GenerateFrame();

        _frameGenerator.GenerationComplete += () => GD.Print("Generation complete");

        _platformGenerator.AddCheckLayer(_frameGenerator.GetTileMapLayer());

        _platformGenerator.SetBounds(LevelSize);
        _platformGenerator.GeneratePlatforms();

        _doorSpawner.levelSize = LevelSize;
        _doorSpawner.Spawn(new Array<Node2D>() {Entrance, Exit}, _platformGenerator.GetTileMapLayer());

        _enemySpawner.levelSize = LevelSize;
        _enemySpawner.Spawn(_platformGenerator.GetTileMapLayer());

        SpawnPlayer();

        Exit.BodyEntered += (body) => _inDoor = true;
        Exit.BodyExited += (body) => _inDoor = false;
    }

    private async void SpawnPlayer()
    {
        if (Entrance != null)
        {
            Node2D player = PlayerScene.Instantiate<Node2D>();
            CallDeferred(Node.MethodName.AddChild, player);
            await ToSignal(player, Node2D.SignalName.Ready);
            player.Position = Entrance.Position;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("ui_up") && _inDoor)
        {
            GetTree().ReloadCurrentScene();
        }
        base._Input(@event);
    }
}


