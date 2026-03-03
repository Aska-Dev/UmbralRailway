using Godot;
using System;

public partial class StartGameLabelArea : Area3D
{
	private Label3D label = null!;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Label3D>("StartGameLabel");
    }

	private void OnMouseEntered()
	{
		label.Text = "Press [E] to Start";
    }
}
