using Godot;
using System;

public enum TrainMotion
{
    Forward,
    Stop,
    Backward
}

public class ComputerData
{
    public bool IsConnectedToNetwork { get; set; } = false;
    public FloppyDisk? InsertedFloppyDisc { get; set; } = null;
}

public partial class Train : Node3D
{
	[Export]
	public Location Location { get; set; } = null!;

    [Export]
    public Directions FacingDirection { get; set; } = Directions.East;

    public TrainMotion Motion { get; set; } = TrainMotion.Stop;
    public ComputerData ComputerData { get; set; } = new ComputerData();
    public Task? CurrentTask { get; set; } = null;

    private AudioStreamPlayer3D dingDongSound = null!;
    private AudioStreamPlayer trainDriving = null!;
    private AudioStreamPlayer trainStopping = null!;
    private AudioStreamPlayer trainStarting = null!;

    public override void _Ready()
    {
        AddToGroup("train");

        TrainEventBus.Instance.TrainLocationTravelled += TravelToLocation;
        TrainEventBus.Instance.TrainMotionChanged += OnTrainMotionChanged;
        TrainEventBus.Instance.TrainDirectionChanged += (TrainDirectionChangedEventArgs args) => FacingDirection = args.NewDirection;
        TrainEventBus.Instance.TrainNetworkConnectionChanged += (bool isConnected) => ComputerData.IsConnectedToNetwork = isConnected;
        TrainEventBus.Instance.TrainFloppyDiscUpdated += (FloppyDisk? floppyDisk) => ComputerData.InsertedFloppyDisc = floppyDisk;
        TrainEventBus.Instance.NewTaskAssigned += (Task task) => CurrentTask = task;

        // Get sound players
        dingDongSound = GetNode<AudioStreamPlayer3D>("Sounds/DingDong");
        trainDriving = GetNode<AudioStreamPlayer>("Sounds/TrainDriving");
        trainStopping = GetNode<AudioStreamPlayer>("Sounds/TrainStopping");
        trainStarting = GetNode<AudioStreamPlayer>("Sounds/TrainStarting");

        TrainEventBus.Instance.TravelToLocation(Location);
    }

    public override void _Process(double delta)
    {
        if(Location is null)
        {
             return;
        }

        if (Motion == TrainMotion.Forward)
        {
            Location.MoveForward(FacingDirection);
        }
        else if (Motion == TrainMotion.Backward)
        {
            Location.MoveBackward(FacingDirection);
        }
    }

    private void TravelToLocation(Location location)
    {
        GD.Print($"Train is traveling to new location: {location}");
        Location = location;

        if(location is TrainStation)
        {
            //TrainEventBus.Instance.ChangeTrainMotion(TrainMotion.Stop);
            dingDongSound.Play();
        }

        // Reset data
        TrainEventBus.Instance.ChangeTrainNetworkConnection(false);
    }

    private void OnTrainMotionChanged(TrainMotionChangedEventArgs motionArgs)
    {
        if(Motion == motionArgs.NewMotion)
        {
            return;
        }

        if (motionArgs.NewMotion == TrainMotion.Stop)
        {
            trainDriving.Stop();
            trainStarting.Stop();

            trainStopping.Play();
        }
        else
        {
            trainDriving.Stop();
            trainStopping.Stop();

            ControlTrainStartingSounds();
        }
        Motion = motionArgs.NewMotion;
    }

    private void ControlTrainStartingSounds()
    {
        trainStarting.Play();

        GetTree().CreateTimer(trainStarting.Stream.GetLength() - 0.2).Timeout += () =>
        {
            if (Motion != TrainMotion.Stop)
            {
                trainDriving.Stop();
                trainDriving.Play();
            }
        };
    }
}
