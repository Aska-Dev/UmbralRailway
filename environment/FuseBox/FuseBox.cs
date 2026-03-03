using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;

public partial class FuseBox : Node3D
{
	[Signal]
	public delegate void FuseBoxMinigameStatusChangedEventHandler(bool isActive);

	[Signal]
	public delegate void OnFuseBoxInputEventHandler(InputEvent @event);

    private List<MeshInstance3D> turner = new List<MeshInstance3D>();
	private FuseBoxMinigameUiGameController minigameUiController = null!;
	private AnimationPlayer animationPlayer = null!;
	private InteractionComponent doorInteractionComponent = null!;
	private Camera3D minigameCamera = null!;

    private bool isDoorOpen = false;
	private bool isMinigameActive = false;

    public override void _Ready()
	{
		for (int i = 1; i <= 9; i++)
		{
			turner.Add(GetNode<MeshInstance3D>($"FuseBoxModel/Model/turner{i}"));
        }

		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        minigameUiController = GetNode<FuseBoxMinigameUiGameController>("MinigameUI/Viewport/FuseBoxMinigameUiGameController");
		doorInteractionComponent = GetNode<InteractionComponent>("FuseBoxDoor/InteractionComponent");
		minigameCamera = GetNode<Camera3D>("Camera3D");

		FuseBoxMinigameStatusChanged += (isActive) =>
		{
			if (isActive)
			{
				StartMinigame();
			}
			else
			{
				EndMinigame();
			}
		};
    }

    public override void _Input(InputEvent @event)
    {
        if (isMinigameActive)
		{
			EmitSignalOnFuseBoxInput(@event);

            if (@event.IsActionPressed(Inputs.Escape) || @event.IsActionPressed(Inputs.Interact))
			{
				EmitSignal(SignalName.FuseBoxMinigameStatusChanged, false);
            }
		}
    }

	public void InteractWithDoor()
	{
		if (isDoorOpen)
		{
			animationPlayer.Play("Close Door");
			isDoorOpen = false;
			doorInteractionComponent.InteractionMessage = "Press [F] to open";

		}
		else
		{
			animationPlayer.Play("Open Door");
			isDoorOpen = true;
			doorInteractionComponent.InteractionMessage = "Press [F] to close";
		}
	}

	public void InteractWithFuseBox()
	{
		if(!isMinigameActive)
		{
			EmitSignal(SignalName.FuseBoxMinigameStatusChanged, true);
        }
	}

	public void OnMinigameTurnerSelected(int turnerIndex)
	{
        turner[turnerIndex].RotationDegrees = new Vector3(0, turner[turnerIndex].RotationDegrees.Y + 90, 0);
    }

	private void StartMinigame()
	{ 	
		isMinigameActive = true; 
		PlayerEventBus.Instance.SetPlayerInputEnabled(false);

		minigameCamera.Current = true;
		UiEventBus.Instance.ShowHintText("Navigate with [W, A, S, D]\nPress [SPACE] to rotate\n\nPress [F] or [ESC] to exit");
    }

	private void EndMinigame()
	{
		isMinigameActive = false;
		PlayerEventBus.Instance.SetPlayerInputEnabled(true);
		PlayerEventBus.Instance.SetPlayerCameraToCurrent();
    }
}
