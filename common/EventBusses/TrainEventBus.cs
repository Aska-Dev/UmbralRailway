using Godot;
using System;
using System.Runtime.Loader;

public partial class TrainMotionChangedEventArgs : RefCounted
{
    public TrainMotion NewMotion { get; }

    public TrainMotionChangedEventArgs(TrainMotion newMotion)
    {
        NewMotion = newMotion;
    }
}

public partial class TrainDirectionChangedEventArgs : RefCounted
{
    public Directions NewDirection { get; }
    public TrainDirectionChangedEventArgs(Directions newDirection)
    {
        NewDirection = newDirection;
    }
}

public partial class TrainEventBus : Node
{
	public static TrainEventBus Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }

    // Location Travel Event
    [Signal]
    public delegate void TrainLocationTravelledEventHandler(Location location);
    public void TravelToLocation(Location location)
    {
        GD.Print($"Emitting TrainLocationTravelled signal for location: {location.ResourcePath}");
        EmitSignal(SignalName.TrainLocationTravelled, location);
    }

    // Motion Change Event
    [Signal]
    public delegate void TrainMotionChangedEventHandler(TrainMotionChangedEventArgs newMotion);
    public void ChangeTrainMotion(TrainMotion newMotion) => EmitSignal(SignalName.TrainMotionChanged, new TrainMotionChangedEventArgs(newMotion));

    // Direction Change Event
    [Signal]
    public delegate void TrainDirectionChangedEventHandler(TrainDirectionChangedEventArgs newDirection);
    public void ChangeTrainDirection(Directions newDirection) => EmitSignal(SignalName.TrainDirectionChanged, new TrainDirectionChangedEventArgs(newDirection));

    // Computer Data Event
    [Signal]
    public delegate void TrainNetworkConnectionChangedEventHandler(bool isConnected);
    public void ChangeTrainNetworkConnection(bool isConnected) => EmitSignal(SignalName.TrainNetworkConnectionChanged, isConnected);

    [Signal]
    public delegate void TrainFloppyDiscUpdatedEventHandler(FloppyDisk? floppyDisk);
    public void UpdateTrainFloppyDisc(FloppyDisk? floppyDisk) => EmitSignal(SignalName.TrainFloppyDiscUpdated, floppyDisk);

    // Task Events
    [Signal]
    public delegate void NewTaskAssignedEventHandler(Task task);
    public void AssignNewTask(Task task) => EmitSignal(SignalName.NewTaskAssigned, task);
}

