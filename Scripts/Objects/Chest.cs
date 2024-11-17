using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;

public partial class Chest : Area2D, ISpawnable
{
	// The textures for the different effects
	[Export]
	public Texture2D HealthTexture = GD.Load<Texture2D>("res://Assets/Sprites/Objects/Items/health.png");
	[Export]
	public Texture2D DamageTexture = GD.Load<Texture2D>("res://Assets/Sprites/Objects/Items/damage.png");
	[Export]
	public Texture2D SpeedTexture = GD.Load<Texture2D>("res://Assets/Sprites/Objects/Items/speed.png");
	[Export]
	public Texture2D JumpTexture = GD.Load<Texture2D>("res://Assets/Sprites/Objects/Items/jump.png");

	// Spawner node
	[Export]
    public Spawn spawner { get; set; }

	// Holds if the chest is locked. TODO make locked by default and allow for unlocking by defeating enemies
	private bool _locked { get; set;} = false;

	// A list of possible effects
	private List<Effect> possibleEffects;

	// The selection options, refered to as slots because they are random
	private Node2D _slots;

	// Animation player
	private AnimationPlayer _animationPlayer;

	// Holds the buttons and their associated effects
	private Dictionary<Button, Effect> _slotEffects;

	// Holds the player node, assigned once the player enters the chest's area
	private Player _player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		// Initializing
		_slots = GetNode<Node2D>("Slots");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_slotEffects = new Dictionary<Button, Effect>();

		// The list of possible effects, might be better moved to a seperate file at a later date
		possibleEffects = new List<Effect>
        {
            new Effect("health", 4.0f, true),
            new Effect("damage", 4.0f, true),
            new Effect("speed", 5.0f, true),
            new Effect("jump", 4.0f, true)
        };

		// Shuffles the effects
		Shuffle<Effect>.ShuffleList(possibleEffects);

		// Loops through the buttons and assigns them effects, icons, tool tips, and connects their signals
		for (int i = 0; i < _slots.GetChildCount(); i++)
		{
			Button button = _slots.GetChild<Button>(i);
			
			_slotEffects.Add(button, possibleEffects[i%_slots.GetChildCount()]);
			 // this is where the issue is. Whenever this is called it moved the iterator to 3 and thus applies the first effect. Fix later
			button.Pressed += () => OnButtonPressed(possibleEffects[i%_slots.GetChildCount()]);

			switch (possibleEffects[i%_slots.GetChildCount()].name)
			{
				case "health":
					button.Icon = HealthTexture;
					button.TooltipText = "Provides a health boost";
					break;
				case "damage":
					button.Icon = DamageTexture;
					button.TooltipText = "Provides a damage boost";
					break;
				case "speed":
					button.Icon = SpeedTexture;
					button.TooltipText = "Provides a speed boost";
					break;
				case "jump":
					button.Icon = JumpTexture;
					button.TooltipText = "Provides a jump boost";
					break;
			}
		}

		// Connects the body entered signal
		BodyEntered += OnChestBodyEntered;
	}

	/// <summary>
	/// Runs when the chest area is entered
	/// </summary>
	/// <param name="body">The body that entered</param>
	public async void OnChestBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player") && body is Player player && !_locked)
		{
			_player = player;
			_animationPlayer.Play("Opening");
			await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
			_animationPlayer.Play("Open");
		}
	}

	/// <summary>
	/// Runs when a button is clicked
	/// </summary>
	/// <param name="effect">The effect to be applied</param>
	public void OnButtonPressed(Effect effect)
	{
		if (_player is Player) // <-- This looks bad, might be bad, but it makes sure the _player variable is acually the player and not something else or null
		{
			_player.AddEffect(effect);
			_animationPlayer.Play("Closing");
		}
	}
}
