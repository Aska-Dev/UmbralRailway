using Godot;
using System;

[GlobalClass]
public partial class SystemDiagnosis : StationService
{
    public override string ServiceName => "SYSTEM_DIAGNOSIS";

    public override void PerformService()
    {
        base.PerformService();

        var tree = Engine.GetMainLoop() as SceneTree;

        SendMessageToInterface("Performing system diagnosis.\nPower will be temporarily shut down. Please wait...", 6);

        tree.CreateTimer(3).Timeout += () =>
        {
            TrainEventBus.Instance.ChangePowerState(false);
            tree.CreateTimer(12).Timeout += () =>
            {
                TrainEventBus.Instance.ChangePowerState(true);
                SendMessageToInterface("System reset was successfully.");
                tree.CreateTimer(3).Timeout += () => TrainEventBus.Instance.CompleteStationServiceRequest();
            };
        };

    }
}
