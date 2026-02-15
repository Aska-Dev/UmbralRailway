using DungeonLetter.Common;
using Godot;
using System;

public enum SpeedLeverStates
{
	Forward,
	Stop,
    Backward
}

public partial class SpeedLever : Node3D, IEntity
{
	public ComponentList Components { get; set; } = null!;

	private SpeedLeverStates _state = SpeedLeverStates.Stop;
	private bool _isActive = false;

    private AnimationTree animationTree = null!;
	private SpeedLeverButtons buttons = null!;

    public override void _Ready()
	{
		animationTree = GetNode<AnimationTree>("AnimationTree");
		buttons = GetNode<SpeedLeverButtons>("SpeedLeverButtons");

        Components = new ComponentList(this);

        // Connect to TrainEventBus to listen for motion changes
		TrainEventBus.Instance.TrainMotionChanged += OnTrainMotionChanged;
    }

    public override void _Input(InputEvent @event)
	{
		if(_isActive)
		{
            if (@event.IsActionPressed(Inputs.MoveForward))
            {
                TrainEventBus.Instance.ChangeTrainMotion(TrainMotion.Forward);
            }

			if(@event.IsActionPressed(Inputs.MoveBack))
			{
				TrainEventBus.Instance.ChangeTrainMotion(TrainMotion.Backward);
            }

			if(@event.IsActionPressed(Inputs.Space))
			{
				TrainEventBus.Instance.ChangeTrainMotion(TrainMotion.Stop);
            }

			if(@event.IsActionPressed(Inputs.Escape) || @event.IsActionPressed(Inputs.Interact))
			{
				SetInactive();
			}
        }
    }

	private void SetActive()
	{
		_isActive = true;
		PlayerEventBus.Instance.SetPlayerInputEnabled(false);
		UiEventBus.Instance.ShowHintText("Press [W] to drive forwards\nPress [SPACE] to stop\nPress [S] to drive backwards\n\nPress [F] to leave the controls");
    }

	private void SetInactive()
	{
		_isActive = false;
		PlayerEventBus.Instance.SetPlayerInputEnabled(true);
		UiEventBus.Instance.ClearHintText();
    }

	private void OnTrainMotionChanged(TrainMotionChangedEventArgs motionArgs)
	{
		var motion = motionArgs.NewMotion;

		if(motion == TrainMotion.Forward)
		{
            animationTree.Set("parameters/blend_position", 1f);
        }
		else if(motion == TrainMotion.Stop)
		{
			animationTree.Set("parameters/blend_position", 0f);
        }
		else if(motion == TrainMotion.Backward)
		{
			animationTree.Set("parameters/blend_position", -1f);
        }

		buttons.UpdateButtonLights(motion);
    }
}
