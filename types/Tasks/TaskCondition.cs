using Godot;
using System;

[GlobalClass]
public partial class TaskCondition : Resource
{
    [Signal] public delegate void ConditionFulfilledEventHandler();

    public bool Fulfilled { get; private set; } = false;

    protected void Complete()
    {
        Fulfilled = true;
        EmitSignal(SignalName.ConditionFulfilled);
    }
}
