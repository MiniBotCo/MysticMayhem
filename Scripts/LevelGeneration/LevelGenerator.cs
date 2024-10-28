using Godot;

public partial class LevelGenerator : Node2D
{
    [ExportGroup("Level Properties")]
    [Export]
    public Vector2I LevelSize { get; set; } = new Vector2I(60, 40);

    private FrameGenerator _frameGenerator;
    private PlatformGenerator _platformGenerator;
    private EnemySpawner _enemySpawner;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _frameGenerator = GetNode<FrameGenerator>("FrameGenerator");
        _platformGenerator = GetNode<PlatformGenerator>("PlatformGenerator");
        _enemySpawner = GetNode<EnemySpawner>("EnemySpawner");

        base._Ready();

        GenerateLevel();
    }

    public void GenerateLevel()
    {

        GD.Randomize();

        _frameGenerator.SetFrameSize(LevelSize);
        _frameGenerator.GenerateFrame();

        _frameGenerator.GenerationComplete += () => GD.Print("Generation complete");
        //await ToSignal(_frameGenerator, FrameGenerator.SignalName.GenerationComplete);

        _platformGenerator.AddCheckLayer(_frameGenerator.GetTileMapLayer());

        _platformGenerator.SetBounds(LevelSize);
        _platformGenerator.GeneratePlatforms();

        //await ToSignal(_platformGenerator, Node.SignalName.Ready);
        _enemySpawner.levelSize = LevelSize;
        _enemySpawner.SpawnEnemies(_platformGenerator.GetTileMapLayer());
    }
}
