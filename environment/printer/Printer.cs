using Godot;
using System;

public partial class Printer : Node3D
{
	private AnimationPlayer animationPlayer = null!;

    public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		TrainEventBus.Instance.NewTaskAssigned += OnNewTaskAssiged;
    }

	private void OnNewTaskAssiged(Task task)
	{
		animationPlayer.Play("Print");
    }
}
