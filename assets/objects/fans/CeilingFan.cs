using Godot;
using System;

public partial class CeilingFan : Node3D
{
	private AnimationPlayer animationPlayer = null!;

    public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("active");

		TrainEventBus.Instance.PowerChanged += OnPowerChanged;
    }

	private void OnPowerChanged(bool hasPower)
	{
		if(hasPower)
		{
			animationPlayer.Play("active");
		}
		else
		{
			animationPlayer.Stop();
		}
    }
}
