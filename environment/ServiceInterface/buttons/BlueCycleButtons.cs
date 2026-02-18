using Godot;
using System;

public partial class BlueCycleButtons : Node3D
{
	private AnimationPlayer animationPlayer = null!;

    public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

	public void PlayPress1Animation()
	{
		animationPlayer.Play("press1");
    }

	public void PlayPress2Animation()
	{
		animationPlayer.Play("press2");
    }
}
