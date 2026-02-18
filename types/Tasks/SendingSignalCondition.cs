using Godot;
using System;

[GlobalClass]
public partial class SendingSignalCondition : TaskCondition
{
    [Export]
    public int RequiredSignal { get; set; } = 0;

    public SendingSignalCondition()
    {
        TrainEventBus.Instance.SignalSended += (int signal) =>
        {
            Complete();
        };
    }
}

