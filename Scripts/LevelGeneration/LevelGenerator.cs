using Godot;

public partial class LevelGenerator : Node2D
{
    [ExportGroup("Level Properties")]
    [Export]
    public Vector2I LevelSize { get; set; } = new Vector2I(60, 40);

    private FrameGenerator _frameGenerator;
    private PlatformGenerator _platformGenerator;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        GD.Randomize();

        _frameGenerator = GetNode<FrameGenerator>("FrameGenerator");
        _platformGenerator = GetNode<PlatformGenerator>("PlatformGenerator");

        _frameGenerator.SetFrameSize(LevelSize);
        _frameGenerator.GenerateFrame();


        _platformGenerator.AddCheckLayer(_frameGenerator.GetTileMapLayer());

        _platformGenerator.SetBounds(LevelSize);
        _platformGenerator.GeneratePlatforms();
    }
}
