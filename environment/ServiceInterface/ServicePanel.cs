using Godot;
using System;
using System.Threading;

public partial class ServicePanel : StaticBody3D, IEntity
{
	public ComponentList Components { set;  get; } = null!;

	[Export] public int PanelIndex { get; set; } = 0;

	private StationService? stationService = null;

	private AnimationPlayer animationPlayer = null!;
    private OmniLight3D readyLamp = null!;
	private OmniLight3D isWorkingLamp = null!;

	private bool isWorking = false;
	private const int blinkTimer = 15;
	private int blinkCooldown = 0;

    public override void _Ready()
	{
		Components = new ComponentList(this);

		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        readyLamp = GetNode<OmniLight3D>("ReadyLamp");
		isWorkingLamp = GetNode<OmniLight3D>("IsWorkingLamp");

        TrainEventBus.Instance.TrainLocationTravelled += OnLocationChange;
    }

    public override void _Process(double delta)
    {
        if(isWorking || isWorkingLamp.Visible)
		{
			BlinkWorkingLamp();
        }
    }

	public void OnInteract()
	{
		animationPlayer.Play("press");
		if(stationService is not null)
		{
			stationService.PerformService();
        }
    }

	private void OnLocationChange(Location location)
	{
		var isActive = false;

        if (location is TrainStation station && station.Data.Services.Length > PanelIndex)
		{
            var service = station.Data.Services[PanelIndex];
            if (service != null && service.IsOperational)
            {
                isActive = true;
				stationService = service;
            }
		}

		UpdateLamp(isActive);
    }

	private void UpdateLamp(bool isActive)
	{
		if(isActive)
		{
			readyLamp.LightColor = Colors.DarkGreen;
        }
		else
		{
			readyLamp.LightColor = Colors.DarkRed;
        }
	}

	private void BlinkWorkingLamp()
	{
		if(blinkCooldown == 0)
		{
			blinkCooldown = blinkTimer;
			isWorkingLamp.Visible = !isWorkingLamp.Visible;
        }
		else
		{
			blinkCooldown--;
        }
    }
}
