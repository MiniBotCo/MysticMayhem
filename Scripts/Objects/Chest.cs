using Godot;
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
	private bool _locked = true; // Holds if the chest is locked
	private List<Effect> possibleEffects; // A list of possible effects
	private Node2D _slots; 	// The selection options, refered to as slots because they are random
	private AnimationPlayer _animationPlayer; // Animation player
	private Dictionary<Button, Effect> _slotEffects; // Holds the buttons and their associated effects
	private Player _player; // Holds the player node, assigned once the player enters the chest's area

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		// Initializing
		_slots = GetNode<Node2D>("Slots");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_slotEffects = new Dictionary<Button, Effect>();
		possibleEffects = new List<Effect>();

		// Applies a random value to the stat for each button
		foreach (Stat stat in Enum.GetValues(typeof(Stat)))
		{
			possibleEffects.Add(new Effect(stat, GD.RandRange(5, 15), true));
		}

		Shuffle<Effect>.ShuffleList(possibleEffects);

		// Loops through the buttons and assigns them effects, icons, tool tips, and connects their signals
		for (int i = 0; i < _slots.GetChildCount(); i++)
		{
			Button button = _slots.GetChild<Button>(i); // Gets the button corrosponding to the slot
			Effect effect = possibleEffects[i%_slots.GetChildCount()]; // Gets the effect corrosponding to the slot
			
			_slotEffects.Add(button, effect); // Adds the button and its effect to the dictionary of slots
			button.Pressed += () => OnButtonPressed(_slotEffects[button]); // Connects the button press signal

			// Sets the proper texture and tooltip
			switch (effect.statResource.StatType)
			{
				case Stat.Health:
					button.Icon = HealthTexture;
					button.TooltipText = "Provides a health boost of ";
					break;
				case Stat.Damage:
					button.Icon = DamageTexture;
					button.TooltipText = "Provides a damage boost of ";
					break;
				case Stat.Speed:
					button.Icon = SpeedTexture;
					button.TooltipText = "Provides a speed boost of ";
					break;
				case Stat.JumpSpeed:
					button.Icon = JumpTexture;
					button.TooltipText = "Provides a jump boost of ";
					break;
			}

			button.TooltipText += effect.statResource.StatValue; // Adds the amount the effect provides to the tool tip
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
			GameStatistics.totalUpgradesGotten++;
			GameStatistics.upgradesGotten++;
			_player.AddEffect(effect);
			_animationPlayer.Play("Closing");
		}
	}

	/// <summary>
	/// Unlocks the chest and plays an animation
	/// </summary>
	public void Unlock()
	{
		_locked = false;
		_animationPlayer.Play("Unlocked");
	}
}
