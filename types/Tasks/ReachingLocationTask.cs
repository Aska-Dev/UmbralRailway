using Godot;
using System;

[GlobalClass]
public partial class ReachingLocationTask : Task
{
    [Export]
    public Location Location { get; set; } = null!;

    public ReachingLocationTask()
    {
        TrainEventBus.Instance.TrainLocationTravelled += (Location location) =>
        {
            if(location == Location)
            {
                var tree = Engine.GetMainLoop() as SceneTree;
                tree!.CreateTimer(3f).Timeout += () =>
                {
                    Complete();
                };
            }
        };
    }
}
