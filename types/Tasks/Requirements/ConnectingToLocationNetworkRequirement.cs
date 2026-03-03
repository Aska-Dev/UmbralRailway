using Godot;
using System;

[GlobalClass]
public partial class ConnectingToLocationNetworkRequirement : TaskRequirement
{
    [Export]
    public Location Location { get; set; } = null!;

    protected override void StartListening()
    {
        TrainEventBus.Instance.TrainNetworkConnectionChanged += OnConnect;
    }

    protected override void StopListening()
    {
        TrainEventBus.Instance.TrainNetworkConnectionChanged -= OnConnect;
    }

    private void OnConnect(bool status)
    {
        var tree = Engine.GetMainLoop() as SceneTree;
        var train = tree!.GetFirstNodeInGroup("train") as Train;

        if (status && Location == train!.Location)
        {
            Complete();
        }
    }
}
