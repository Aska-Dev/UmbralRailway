using Godot;
using System;

public partial class ServiceInterfaceDiskSpace : Node3D
{
	private AnimationPlayer animationPlayer = null!;

    public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

	public void PlayDiskInputAnimation()
	{
		animationPlayer.Play("input");
    }

	public void PlayDiskEjectAnimation()
	{
		animationPlayer.Play("eject");
    }
}
