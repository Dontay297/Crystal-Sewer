using Godot;
using System;
using System.Collections.ObjectModel;

public partial class Orb : Area3D
{
	public bool PlayerInRange = false;
	private Label3D collectLabel;
	public Stickman PlayerRef;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
		collectLabel = GetNode<Label3D>("Label3D");
		collectLabel.Visible = false;
	}

    private void OnBodyExited(Node3D body)
    {
		if (body is Stickman player)
		{
			PlayerInRange = false;
			if (player.CurrentOrb == this)
				player.CurrentOrb = null;
			collectLabel.Visible = false;
		}
    }


    private void OnBodyEntered(Node3D body)
    {
		if (body is Stickman player)
		{
			PlayerInRange = true;
			PlayerRef = player;
			player.CurrentOrb = this;
			collectLabel.Visible = true;
		}
    }

	public void Collect()
    {
		var GameManager = GetTree().Root.GetNode<GameManager>("Main/Game Manager");
		if (GameManager != null)
		{
			GameManager.OnOrbCollected();
		}
		else
		{
			GD.PrintErr("GameManager is null!");
		}
		QueueFree();
	}
    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
