using Godot;
using System;

public partial class SpeedLeverButtons : Node3D
{
	private OmniLight3D forwardLight = null!;
    private OmniLight3D backwardLight = null!;

	public override void _Ready()
	{
		forwardLight = GetNode<OmniLight3D>("ForwardLight");
		backwardLight = GetNode<OmniLight3D>("BackwardLight");
	}

	public void UpdateButtonLights(TrainMotion motion)
	{
		forwardLight.Visible = motion == TrainMotion.Forward;
		backwardLight.Visible = motion == TrainMotion.Backward;
	}
}
