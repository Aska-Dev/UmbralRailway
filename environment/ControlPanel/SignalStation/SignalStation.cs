using DungeonLetter.Common;
using Godot;
using System;

public partial class SignalStation : Node3D, IEntity
{
	public ComponentList Components { get; set; } = null!;

	private bool isActive = false;

	private int signal = 0;
	private const int MaxSignal = 25;

    private MeshInstance3D turner = null!;
	private Label3D display = null!;

	private AudioStreamPlayer clickSound = null!;
	private AudioStreamPlayer successSound = null!;
	private OmniLight3D signalLight = null!;

    public override void _Ready()
	{
		Components = new ComponentList(this);

		turner = GetNode<MeshInstance3D>("Turner");
		display = GetNode<Label3D>("Display");
		clickSound = GetNode<AudioStreamPlayer>("ClickSound");
		successSound = GetNode<AudioStreamPlayer>("SuccessSound");
		signalLight = GetNode<OmniLight3D>("light");
    }

    public override void _Input(InputEvent @event)
    {
        if(isActive)
		{
			if(@event.IsActionPressed(Inputs.Interact))
			{
				isActive = false;
				display.Visible = false;
				PlayerEventBus.Instance.SetPlayerInputEnabled(true);
				UiEventBus.Instance.ClearHintText();
            }

			if(@event.IsActionPressed(Inputs.MoveRight))
			{
				CycleRight();
			}

			if(@event.IsActionPressed(Inputs.MoveLeft))
			{
				CycleLeft();
            }

			if(@event.IsActionPressed(Inputs.Space))
			{
				SendSignal();
			}
        }
    }

	public void OnInteract()
	{
		isActive = true;
		display.Visible = true;

		PlayerEventBus.Instance.SetPlayerInputEnabled(false);
		UiEventBus.Instance.ShowHintText("Press [A] or [D] to cycle through signals\nPress [SPACE] to confirm signal\n\nPress [F] to leave the station");
    }

	private void SendSignal()
	{
		TrainEventBus.Instance.SendSignal(signal);
        successSound.Play();

		signalLight.Visible = true;
		GetTree().CreateTimer(0.25f).Timeout += () => signalLight.Visible = false;
    }

	private void CycleRight()
	{
		if(signal == MaxSignal)
		{
			return;
		}

		signal++;
		UpdateTurnerRotationAndLabel();
    }

	private void CycleLeft()
	{
		if(signal == 0)
		{
			return;
        }

		signal--;
		UpdateTurnerRotationAndLabel();
    }

	private void UpdateTurnerRotationAndLabel()
	{
		display.Text = $"10-{signal}";

        float rotationAngle = 125 - (signal * 10);
		turner.RotationDegrees = new Vector3(0, rotationAngle, 0);

		clickSound.Play();
    }
}
