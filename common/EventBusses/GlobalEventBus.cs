using Godot;
using System;

public partial class GlobalEventBus : Node
{
    public static GlobalEventBus Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }

    [Signal]
    public delegate void ComputerAnimationTriggeredEventHandler(string animationName);
    public void TriggerComputerAnimation(string animationName) => EmitSignal(SignalName.ComputerAnimationTriggered, animationName);
}
