using Godot;
using System;

public partial class SettingsMenu : Control
{
	
	private HSlider _volume;
	private HSlider _difficulty;
	private CheckBox _fullScreen;
	private CheckBox _borderlessWindowed;
	private Button _exitButton;
	public override void _Ready()
	{
		_volume = GetNode<HSlider>("CenterBox/VBoxContainer/VolumeSlider");
		_difficulty = GetNode<HSlider>("CenterBox/VBoxContainer/DifficultySlider");
		_fullScreen = GetNode<CheckBox>("CenterBox/VBoxContainer/HBoxContainer/FullScreen");
		_borderlessWindowed = GetNode<CheckBox>("CenterBox/VBoxContainer/HBoxContainer2/BorderlessWindow");
		_exitButton = GetNode<Button>("HBoxContainer/ExitButton");

		_volume.Value = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Master"));

		_difficulty.Value = GameStatistics.difficulty;

		if(DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
		{
			_fullScreen.ButtonPressed = true;
		}

		if(DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless))
		{
			_borderlessWindowed.ButtonPressed = true;
		}

		_volume.ValueChanged += ChangeVolume;
		_difficulty.ValueChanged += ChangeDifficulty;
		_fullScreen.Toggled += ToggleFullScreen;
		_borderlessWindowed.Toggled += ToggleBorderlessWindowed;
		_exitButton.Pressed += ExitToMainMenu;
	}

	private void ChangeVolume(double volume)
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), (float) volume);
	}

	private void ChangeDifficulty(double difficulty)
	{
		GameStatistics.difficulty = (int) difficulty;
	}

	private void ToggleFullScreen(bool fullScreened)
	{
		if (fullScreened)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			_borderlessWindowed.Disabled = true;
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			_borderlessWindowed.Disabled = false;
		}
	}

	private void ToggleBorderlessWindowed(bool borderlessWindowed)
	{
		if (borderlessWindowed)
		{
			DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
			_fullScreen.Disabled = true;
		}
		else
		{
			DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
			_fullScreen.Disabled = false;
		}
	}

	private void ExitToMainMenu()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Main Menu.tscn");
	}
}
