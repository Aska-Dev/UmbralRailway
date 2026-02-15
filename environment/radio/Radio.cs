using Godot;
using System;

public partial class Radio : Node3D
{
    private OmniLight3D light = null!;

    public override void _Ready()
	{
		light = GetNode<OmniLight3D>("Light");
		UpdateLight(true);
    }

	private void UpdateLight(bool lightStatus)
	{
		light.Visible = lightStatus;
    }
}
