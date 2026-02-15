using Godot;
using System;

public partial class PowerGauge : Node3D
{
	private AnimationPlayer animationPlayer = null!;

    public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        TrainEventBus.Instance.TrainMotionChanged += OnTrainMotionChanged;
    }

	private void OnTrainMotionChanged(TrainMotionChangedEventArgs args)
	{
		var newMotion = args.NewMotion;
		if(newMotion == TrainMotion.Forward || newMotion == TrainMotion.Backward)
		{
			animationPlayer.Play("Moving");
        }
		else
		{
			animationPlayer.Play("RESET");
        }
    }
}
