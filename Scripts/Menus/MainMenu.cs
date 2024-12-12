using Godot;
using System;

public partial class MainMenu : Control
{
	private Button _startGame;
	private Button _settings;
	private Button _exitGame;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startGame = GetNode<Button>("CenterContainer/VBoxContainer/Start");
		_settings = GetNode<Button>("CenterContainer/VBoxContainer/Settings");
		_exitGame = GetNode<Button>("CenterContainer/VBoxContainer/Quit");

		_startGame.Pressed += OnStartGamePressed;
		_settings.Pressed += OnSettingsPressed;
		_exitGame.Pressed += OnSettingsPressed;
	}

	private void OnStartGamePressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Levels/Level.tscn");
	}

	private void OnSettingsPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Menus/settings_menu.tscn");
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}