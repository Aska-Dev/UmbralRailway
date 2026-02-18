using Godot;
using System;

public partial class RequestStationServiceButton : Node3D
{
	private AnimationPlayer animationPlayer = null!;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

	public void PlayPressAimation()
	{
		animationPlayer.Play("Press");
    }
}
