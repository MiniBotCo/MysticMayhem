using Godot;
using System;

public partial class StatisticsMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Label>("CenterContainer/VBoxContainer/HighestLevel").Text = "Highest Level: " + GameStatistics.highestLevel;
		GetNode<Label>("CenterContainer/VBoxContainer/TotalLevelsCleared").Text = "Total Levels Cleared: " + GameStatistics.totalLevelsCleared;
		GetNode<Label>("CenterContainer/VBoxContainer/EnemiesDefeated").Text = "Total Enemies Defeated: " + GameStatistics.totalEnemiesDefeated;
		GetNode<Label>("CenterContainer/VBoxContainer/UpgradesGotten").Text = "Upgrades Gotten: " + GameStatistics.totalUpgradesGotten;
		GetNode<Label>("CenterContainer/VBoxContainer/Deaths").Text = "Total Deaths: " + GameStatistics.deaths;

		GetNode<Button>("ExitToMenu").Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/Main Menu.tscn");
	}
}
