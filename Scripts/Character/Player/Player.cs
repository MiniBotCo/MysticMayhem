using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

public partial class Player : Character
{
	[ExportGroup("Player Stats")]
	[Export(PropertyHint.Range, "50,800,25")] public float PlayerSpeed = 200.0f;
	[Export(PropertyHint.Range, "-850,-50,25")] public float JumpVelocity = -450.0f;
	[Export] public float Health = 20.0f;
	[Export] public float Damage = 5.0f;

	public Vector2 direction = new();


	private Control _hud;
	private static List<Effect> _effects = new List<Effect>();

	public AudioStreamWav jumpSound = GD.Load<AudioStreamWav>("res://Assets/Sfx/jump.wav");
	public AudioStreamWav landSound = GD.Load<AudioStreamWav>("res://Assets/Sfx/land.wav");
    public AudioStreamWav swordSwingSound = GD.Load<AudioStreamWav>("res://Assets/Sfx/swordSwing.wav");

	public override void _Ready()
	{
		_hud = GetNode<Control>("HUDCanvasLayer/HUD");
		ApplyEffects();
		UpdateHUD();
		characterNode = GetOwner<Player>();
	}



	/// <summary>
	/// Applies the effects to the player
	/// </summary>
	private void ApplyEffects()
	{
		List<Effect> nonPermanentEffects = new List<Effect>();
		foreach (Effect effect in _effects)
		{
			switch(effect.name)
			{
				case "health":
					Health += effect.amount;
					break;
				case "damage":
					Damage += effect.amount;
					break;
				case "speed":
					PlayerSpeed += effect.amount;
					break;
				case "jump":
					JumpVelocity -= effect.amount;
					break;
				default:
					break;
			}

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

	/// <summary>
	/// Adds an effect to the effects list
	/// </summary>
	/// <param name="effect">The effect to be added</param>
	public void AddEffect(Effect effect)
	{
		_effects.Add(effect);
	}

	private void UpdateHUD()
	{
		_hud.GetNode<Label>("HBoxContainer/Health").Text = "Health: " + Health;
		_hud.GetNode<Label>("HBoxContainer/Damage").Text = "Damage: " + Damage;
		_hud.GetNode<Label>("HBoxContainer/Jump").Text = "Jump: " + JumpVelocity;
		_hud.GetNode<Label>("HBoxContainer/Speed").Text = "Speed: " + PlayerSpeed;
	}
}
