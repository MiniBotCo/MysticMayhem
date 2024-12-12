using Godot;
using System;

public partial class DeathScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Label>("CenterContainer/VBoxContainer/FinalLevel").Text = "Final Level: " + GameStatistics.finalLevel;
		GetNode<Label>("CenterContainer/VBoxContainer/EnemiesDefeated").Text = "Enemies Defeated: " + GameStatistics.enemiesDefeated;
		GetNode<Label>("CenterContainer/VBoxContainer/UpgradesGotten").Text = "Upgrades Gotten: " + GameStatistics.upgradesGotten;

		GameStatistics.ResetRunStatistics();

		GetNode<Button>("ExitToMenu").Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/Main Menu.tscn");
	}
}
