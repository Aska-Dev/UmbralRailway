using DungeonLetter.Common;
using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

public partial class ServiceInterface : StaticBody3D, IEntity
{
	public ComponentList Components { get; set; } = null!;

	private bool isActive = false;
	private bool isConnected = false;
    private bool requestOngoing = false;
    private int selectedServiceIndex = 0;
	private Location? currentLocation = null;
	
    private Label3D title = null!;
	private Label3D selectedService = null!;
	private Label3D note = null!;

	private AudioStreamPlayer pressSound = null!;
	private AudioStreamPlayer errorSound = null!;
	private AudioStreamPlayer3D workingSound = null!;

    public override void _Ready()
	{
		Components = new ComponentList(this);

		pressSound = GetNode<AudioStreamPlayer>("PressSound");
		errorSound = GetNode<AudioStreamPlayer>("ErrorSound");
		workingSound = GetNode<AudioStreamPlayer3D>("WorkingSound");

        title = GetNode<Label3D>("Title");
		selectedService = GetNode<Label3D>("SelectedService");
		note = GetNode<Label3D>("Note");

		TrainEventBus.Instance.TrainNetworkConnectionChanged += OnConnectionChanged;
		TrainEventBus.Instance.TrainLocationTravelled += (Location location) => currentLocation = location;
    }

    public override void _Input(InputEvent @event)
    {
        if(isActive)
		{
			if(@event.IsActionPressed(Inputs.Interact))
			{
				Disable();
			}

			if(!requestOngoing)
			{
                if (@event.IsActionPressed(Inputs.MoveRight))
                {
					pressSound.Play();
                    CycleServicesForward();
                }

                if (@event.IsActionPressed(Inputs.MoveLeft))
                {
					pressSound.Play();
                    CycleServicesBackward();
                }

                if (@event.IsActionPressed(Inputs.Space))
                {
					pressSound.Play();
                    RequestSelectedService();
                }
            }
        }
    }

	public void Enable()
	{
		isActive = true;
        PlayerEventBus.Instance.SetPlayerInputEnabled(false);
        UiEventBus.Instance.ShowHintText("Press [A] or [D] to cycle between services\nPress [SPACE] to request the selected service\n\nPress [F] to leave the station");
    }

	private void Disable()
	{
		isActive = false;
		PlayerEventBus.Instance.SetPlayerInputEnabled(true);
		UiEventBus.Instance.ClearHintText();
    }

	private void OnConnectionChanged(bool connectionStatus)
	{
		if(isConnected == connectionStatus)
		{
            return;
		}

		isConnected = connectionStatus;
        selectedServiceIndex = 0;

        if (isConnected && currentLocation is TrainStation station)
		{
            if(station.Data.Services.Length > 0)
			{
				title.Text = $"Select a station service:";
				selectedService.Text = $"[{selectedServiceIndex}]\n{station.Data.Services[selectedServiceIndex].ServiceName}";
				note.Text = "";
            }
			else
			{
				title.Text = "No Services";
				selectedService.Text = "N/A";
				note.Text = "This station does not provide any available services.";
            }

			return;
        }

        title.Text = "No Connection";
        selectedService.Text = "N/A";
        note.Text = "Please connect to a valid network.";
    }

	private void RequestSelectedService()
	{
        if (isConnected && currentLocation is TrainStation station)
		{
			requestOngoing = true;
            var service = station.Data.Services[selectedServiceIndex];
			if (service.IsOperational)
			{
				workingSound.Play();

                note.Text = $"Requested service. Waiting for response...";
				GetTree().CreateTimer(4).Timeout += () =>
				{
					note.Text = $"Station confirmed request. Processing...\n[       0%     ]";
					GetTree().CreateTimer(1).Timeout += () =>
					{
						note.Text = $"Station confirmed request. Processing...\n[███   17%     ]\"";
						GetTree().CreateTimer(2).Timeout += () =>
						{
							note.Text = $"Station confirmed request. Processing...\n[██████ 50%     ]\"";
							GetTree().CreateTimer(3).Timeout += () =>
							{
								note.Text = $"Station confirmed request. Processing...\n[██████████ 100%]\"";
								GetTree().CreateTimer(1).Timeout += () =>
								{
									note.Text = $"Service execution complete.";
									service.PerformService();
									requestOngoing = false;

									GetTree().CreateTimer(2).Timeout += () => note.Text = "";
                                };
							};
						};
                    };
                };
            }
			else
			{
                errorSound.Play();
				note.Text = "";

				GetTree().CreateTimer(0.5).Timeout += () =>
				{
                    note.Text = "This service is currently not operational.";
					GetTree().CreateTimer(0.5).Timeout += () =>
					{
                        note.Text = "";
						GetTree().CreateTimer(0.5).Timeout += () =>
						{
							note.Text = "This service is currently not operational.";
							requestOngoing = false;
						};
                    };
                };
            }
		}
    }

    private void CycleServicesForward()
	{
		if(isConnected && currentLocation is TrainStation station)
		{
			if (selectedServiceIndex + 1 < station.Data.Services.Length)
			{
				selectedServiceIndex++;
			}
			else
			{
				selectedServiceIndex = 0;
			}

			UpdateLabel();
        }
	}

    private void CycleServicesBackward()
    {
        if (isConnected && currentLocation is TrainStation station)
        {
            if (selectedServiceIndex - 1 > -1)
            {
                selectedServiceIndex--;
            }
            else
            {
                selectedServiceIndex = station.Data.Services.Length -1;
            }

			UpdateLabel();
        }
    }

	private void UpdateLabel()
	{
		if (currentLocation is TrainStation station)
		{
			selectedService.Text = $"[{selectedServiceIndex}]\n{station.Data.Services[selectedServiceIndex].ServiceName}";
			if (station.Data.Services[selectedServiceIndex].IsOperational)
			{
				note.Text = "";
			}
			else
			{
				note.Text = "This service is currently not operational.";
			}
		}
    }
}