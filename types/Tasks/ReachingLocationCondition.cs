using Godot;
using System;

[GlobalClass]
public partial class ReachingLocationCondition : TaskCondition
{
    [Export]
    public Location Location { get; set; } = null!;

    public ReachingLocationCondition()
    {
        TrainEventBus.Instance.TrainLocationTravelled += (Location location) =>
        {
            if(location == Location)
            {
                Complete();
            }
        };
    }
}
