using Godot;
using System;

public partial class Level : Node2D
{
	[Export]
	public PackedScene levelGeneratorScene { get; set; } = GD.Load<PackedScene>("res://Scenes/level_generator.tscn");
	public int level = 0;

	private bool _inDoor = false;
    private AnimationPlayer _animationPlayer;
	private LevelGenerator _levelGenerator;

	private Door _exit;
	private Door _entrance;
	private Player _player;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	    _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		AddLevelGenerator();
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
            _levelGenerator.QueueFree();
			await ToSignal(_levelGenerator, Node.SignalName.TreeExited);
			AddLevelGenerator();
			_animationPlayer.Play("EnterScene");
			await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
        }
        
        base._Input(@event);
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

	private async void AddLevelGenerator()
	{
		_levelGenerator = levelGeneratorScene.Instantiate<LevelGenerator>();
		CallDeferred(Node.MethodName.AddChild, _levelGenerator);
		await ToSignal(_levelGenerator, Node.SignalName.Ready);

		_entrance = _levelGenerator.Entrance;
		_exit = _levelGenerator.Exit;
		_player = _levelGenerator.Player;

		level++;
		_player.level = level;
		_player.UpdateHUD();

		// Signals for the exit door, used to exit the level
        _exit.BodyEntered += OnExitBodyEntered;
        _exit.BodyExited += OnExitBodyExited;

		
		_levelGenerator.GenerateLevel(level);
	}
}
