using Godot;
using System;

[GlobalClass]
public partial class ShuntingCrane : StationService
{
    [Export] public Wagon? AvailableWagon { get; set; } = null;

    public override string ServiceName => "SHUNTING_CRANE";

    private PackedScene craneMovingSoundScene = GD.Load<PackedScene>("res://environment/SoundScenes/ConnectingWagon.tscn");

    public override void PerformService()
    {
        base.PerformService();

        var tree = Engine.GetMainLoop() as SceneTree;

        if (AvailableWagon is null)
        {
            SendMessageToInterface("No wagon is currently available.");
            tree!.CreateTimer(3).Timeout += () =>
            {
                TrainEventBus.Instance.CompleteStationServiceRequest();
            };

            return;
        }

        SendMessageToInterface("Shunting crane is moving the wagon. Please wait...");

        var craneMovingSound = craneMovingSoundScene.Instantiate<AudioStreamPlayer>();
        tree!.Root.AddChild(craneMovingSound);
        craneMovingSound.Play();

        tree.CreateTimer(10).Timeout += () =>
        {
            SendMessageToInterface("Shunting crane has successfully moved the wagon.");

            TrainEventBus.Instance.UpdateWagon(AvailableWagon);
            AvailableWagon = null;

            craneMovingSound.QueueFree();
            tree.CreateTimer(3).Timeout += () =>
            {
                TrainEventBus.Instance.CompleteStationServiceRequest();
            };
        };
    }
}
