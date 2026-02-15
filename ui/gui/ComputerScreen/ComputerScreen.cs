using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;

public partial class ComputerScreen : Control
{
	private LineEdit userInput = null!;
	private RichTextLabel display = null!;
	private AudioStreamPlayer crtHummingSound = null!;

	private Location? currentLocation = null;
	private ComputerData computerData = new ComputerData();

	private string DiscNotice => $"Disc Detected: {computerData.InsertedFloppyDisc!.Name}\n\nAvailable commands:\n\nread - Read content\neject - eject disc from computer";

    public override void _Ready()
	{
        userInput = GetNode<LineEdit>("UserInput");
		display = GetNode<RichTextLabel>("Display");
		crtHummingSound = GetNode<AudioStreamPlayer>("HummingSound");

        // Start the CRT humming sound
        crtHummingSound.Play();

        // Focus the userInput
        userInput.GrabFocus();
		userInput.FocusExited += OnUserInputFocusExited;

		// Get current location
		var train = GetTree().GetFirstNodeInGroup("train") as Train;
		if (train != null)
		{
			currentLocation = train.Location;
            computerData = train.ComputerData;
        }

		if(computerData.InsertedFloppyDisc is not null)
		{
			PrintLine(DiscNotice);
        }

		if(computerData.IsConnectedToNetwork)
		{
			PrintLine(GetStationNotice());
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb && mb.Pressed)
        {
            if (mb.ButtonIndex == MouseButton.WheelUp || mb.ButtonIndex == MouseButton.WheelDown)
            {
                var vscroll = display.GetVScrollBar();
                if (vscroll != null && vscroll.Visible)
                {
                    // Schrittweite anpassen, je nach Bedarf
                    var step = vscroll.Page / 8.0; // Viertel Seite scrollen
                    if (mb.ButtonIndex == MouseButton.WheelDown)
                        vscroll.Value = Math.Min(vscroll.MaxValue, vscroll.Value + step);
                    else
                        vscroll.Value = Math.Max(vscroll.MinValue, vscroll.Value - step);
                }
            }
        }
    }

	public void HandleUserInput(string input)
	{
		userInput.Text = "";

        switch (input)
		{
			case "help":
				Help();
				break;
			case "exit":
				Exit();
				break;
			case "connect":
				Connect();
				break;
			case "status":
				Status();
                break;
			case "read":
				Read();
				break;
			case "eject":
				Eject();
				break;
			case "logs":
				Logs();
				break;
            default:
                PrintLine($"Unknown command: {input}\nType 'help' for a list of commands.");
				break;
        }
    }

	private void OnLocationChanged(Location newLocation)
	{
		currentLocation = newLocation;
		PrintLine($"Location changed: {currentLocation.ResourceName}");
    }

    private void PrintLine(string text)
	{
		display.AppendText(text + "\n\n");
    }

	private void OnUserInputFocusExited()
	{
		// Refocus the userInput when it loses focus
		userInput.GrabFocus();
	}

    private string GetStationNotice()
	{
		if(currentLocation is TrainStation station)
		{
			return $"Connected to station network: {station.Name} (Id: {station.Id})\n\nAvailable commands:\n\nstatus - Get station status\nlogs - Read latest station log";
		}

		return "";
    }

	// COMMANDS

	private void Read()
	{
		if(computerData.InsertedFloppyDisc is not null)
		{
			PrintLine($"Reading from {computerData.InsertedFloppyDisc.Name}...\n\n{computerData.InsertedFloppyDisc.Data}");
		}
		else
		{
			PrintLine("No disk found");
        }
    }

	private void Eject()
	{
		if(computerData.InsertedFloppyDisc is not null)
		{
            UiEventBus.Instance.ToggleComputerScreen(false);
            GlobalEventBus.Instance.TriggerComputerAnimation("EjectDisk");

            GetTree().CreateTimer(1.8f).Timeout += () =>
            {
                PlayerEventBus.Instance.SetPlayerInputEnabled(true);
                PlayerEventBus.Instance.PickUpDiskFromComputer();
            };
        }
		else
		{
			PrintLine("No disk found");
		}
    }

    private void Help()
	{
		PrintLine("Available commands:\nhelp - Show this help message\nconnect - Connect to the closest available network\nexit - Exit the computer screen");
    }

	private void Connect()
	{
		PrintLine("Attempting to connect to the closest network...");
        
		if(currentLocation is TrainStation station)
		{
            GetTree().CreateTimer(1f).Timeout += () =>
            {
				PrintLine(GetStationNotice());
				TrainEventBus.Instance.ChangeTrainNetworkConnection(true);
				computerData.IsConnectedToNetwork = true;
            };

			return;
        }

        GetTree().CreateTimer(1f).Timeout += () =>
        {
            PrintLine("No network available.");
        };

		return;
    }

	private void Status()
	{
		if(currentLocation is TrainStation station && computerData.IsConnectedToNetwork)
		{
			var temperature = station.Data.TemperatureSensorWorking ? $"{station.Data.Temperature}°C" : "N/A";

			var info = $"{station.Name} - Id: {station.Id}\n" +
                "--------------------------------------------------\n" +
                $"Outside temperature: {temperature}\n" +
				$"Active Power Generators: {station.Data.WorkingPowerGenerators}/3\n" +
				$"--------------------------------------------------\n" +
				$"Available Station Services\n" +
				$"--------------------------------------------------";

			if(station.Data.Services.Length <= 0)
			{
				info += "\nNO SERVICES";
            }

            foreach (var service in station.Data.Services)
            {
				var status = service.IsOperational ? "ONLINE" : service.IsFunctional ? "OFFLINE" : "MALFUNCTION DETECTED";
				info += ("\n" + service.ServiceName + $" [{status}]");
            }

            PrintLine(info);
        }
		else
		{
			PrintLine("Please connect to a station network first.");
        }
    }

	private void Logs()
	{
		if(currentLocation is TrainStation station && computerData.IsConnectedToNetwork)
		{
			var logContent = station.Data.LatestLog;
			PrintLine(logContent);
        }
		else
		{
			PrintLine("Please connect to a station network first.");
        }
    }

    private void Exit()
	{
		UiEventBus.Instance.ToggleComputerScreen(false);
		PlayerEventBus.Instance.SetPlayerInputEnabled(true);
    }
}
