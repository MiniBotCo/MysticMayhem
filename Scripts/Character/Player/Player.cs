using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

public partial class Player : Character
{
	[ExportGroup("Player Stats")]
	[Export] public int level = 0;

	public Vector2 direction = new();


	private Control _hud;
	private static List<Effect> _effects = new List<Effect>();

	public AudioStreamWav jumpSound = GD.Load<AudioStreamWav>("res://Assets/Sfx/jump.wav");
	public AudioStreamWav landSound = GD.Load<AudioStreamWav>("res://Assets/Sfx/land.wav");
	public AudioStreamWav swordSwingSound = GD.Load<AudioStreamWav>("res://Assets/Sfx/swordSwing.wav");

    [Signal]
    public delegate void DeathEventHandler();

	public override void _Ready()
	{
		base._Ready();

        // Stats have to be placed here for now. I don't know why yet
        stats = new StatResource[]
		{
			new StatResource(Stat.Health, 100),
			new StatResource(Stat.Damage, 20),
			new StatResource(Stat.Speed, 200),
			new StatResource(Stat.JumpSpeed, 600)
		};

		_hud = GetNode<Control>("HUDCanvasLayer/HUD");
		ApplyEffects();
		characterNode = this;
	}

	/// <summary>
	/// Applies the effects to the player
	/// </summary>
	private void ApplyEffects()
	{
		List<Effect> nonPermanentEffects = new List<Effect>();
		foreach (Effect effect in _effects)
		{
			GetStatResource(effect.statResource.StatType).StatValue += effect.statResource.StatValue;

			if (!effect.permanent)
			{
				nonPermanentEffects.Add(effect);
			}

			//GD.Print(effect.name); //TODO remove
		}

		foreach (Effect effect in nonPermanentEffects)
		{
			if (!effect.permanent) _effects.Remove(effect);
		}

		//GD.Print("Stats: Health " + Health + " | Damage " + Damage + " | Speed " + PlayerSpeed + " | Jump Speed " + JumpVelocity); //TODO remove
	}

	protected override void HandleHurtboxEntered(Area2D area)
    {
        StatResource health = GetStatResource(Stat.Health);
        Character attacker = area.GetOwner<Character>();
        health.StatValue -= attacker.GetStatResource(Stat.Damage).StatValue;
		UpdateHUD();
    }


	/// <summary>
	/// Adds an effect to the effects list
	/// </summary>
	/// <param name="effect">The effect to be added</param>
	public void AddEffect(Effect effect)
	{
		_effects.Add(effect);
	}
 
	public void UpdateHUD()
	{
		_hud.GetNode<Label>("HBoxContainer/Health").Text = "Health: " + GetStatResource(Stat.Health).StatValue;
		_hud.GetNode<Label>("HBoxContainer/Damage").Text = "Damage: " + GetStatResource(Stat.Damage).StatValue;
		_hud.GetNode<Label>("HBoxContainer/Jump").Text = "Jump: " + GetStatResource(Stat.JumpSpeed).StatValue;
		_hud.GetNode<Label>("HBoxContainer/Speed").Text = "Speed: " + GetStatResource(Stat.Speed).StatValue;
		_hud.GetNode<Label>("HBoxContainer2/Level").Text = "Level: " + level;
	}

	public void ClearEffects()
	{
		_effects.Clear();
	}
}
