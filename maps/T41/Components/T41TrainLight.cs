using Godot;
using System;

public partial class T41TrainLight : OmniLight3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TrainEventBus.Instance.PowerChanged += (bool hasPower) => Visible = hasPower;
    }
}
