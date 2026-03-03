using Godot;
using System;

[GlobalClass]
public partial class ReachingLocationRequirement : TaskRequirement
{
    [Export]
    public Location Location { get; set; } = null!;

    protected override void StartListening()
    {
        TrainEventBus.Instance.TrainLocationTravelled += OnTrainLocationTravelled;
    }

    protected override void StopListening()
    {
        TrainEventBus.Instance.TrainLocationTravelled -= OnTrainLocationTravelled;
    }

    private void OnTrainLocationTravelled(Location location)
    {
        if (location == Location)
        {
            Complete();
        }
    }
}
