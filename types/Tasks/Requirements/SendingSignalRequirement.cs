using Godot;
using System;

[GlobalClass]
public partial class SendingSignalRequirement : TaskRequirement
{
    [Export]
    public int RequiredSignal { get; set; } = 0;

    protected override void StartListening()
    {
        TrainEventBus.Instance.SignalSended += OnSignalSent;
    }

    protected override void StopListening()
    {
        TrainEventBus.Instance.SignalSended -= OnSignalSent;
    }

    private void OnSignalSent(int signal)
    {
        if (signal == RequiredSignal)
        {
            Complete();
        }
    }
}

