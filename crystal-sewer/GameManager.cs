using Godot;
using System;

public partial class GameManager : Node
{

	//Game manager will manage the pause menu, Orbs collected and update UI
	//Variables to track
	private int _totalOrbs = 8;
	private int _collectedOrbs = 0;
	[Export] private Label _orbLabel;

	//Variables that will be used for the pause menu
	private Control _pauseMenu;
	private Button _resumeButton;
	private Button _quitButton;

	public void OnOrbCollected()
	{
		_collectedOrbs++;
		updateUI();

		if (_collectedOrbs >= _totalOrbs)
		{
			GD.Print("All orbs collected");
		}
	}


	private void updateUI()
	{
		_orbLabel.Text = $"Orbs: {_collectedOrbs} / {_totalOrbs}";
	}

	public void StartGame()
	{

	}

	public void EndGame()
	{

	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_totalOrbs = GetTree().GetNodesInGroup("orbs").Count;
		_orbLabel = GetNodeOrNull<Label>("UI/HUD/OrbCounter");
		updateUI();

		_pauseMenu = GetNode<Control>("UI/Pause Menu");
		_pauseMenu.Visible = false;

		_resumeButton = GetNode<Button>("UI/Pause Menu/Resume");
		_quitButton = GetNode<Button>("UI/Pause Menu/Quit");

		_resumeButton.Pressed += OnResumePressed;
		_quitButton.Pressed += OnQuitPressed;

	}

	private void TogglePause()
	{
		if (GetTree().Paused)
		{
			GetTree().Paused = false;
			_pauseMenu.Visible = false;
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
		else
		{
			GetTree().Paused = true;
			_pauseMenu.Visible = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}


	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			TogglePause();
		}
	}

	private void OnResumePressed()
	{
		TogglePause();
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	

}
