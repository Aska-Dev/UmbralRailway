using Godot;
using System;

public partial class Printer : Node3D
{
	private AnimationPlayer animationPlayer = null!;

    public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		TrainEventBus.Instance.MessagePrinted += OnMessagePrinted;
    }

	private void OnMessagePrinted(NoteContent content)
	{
		animationPlayer.Play("Print");
    }
}
