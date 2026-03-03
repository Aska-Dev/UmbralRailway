using Godot;
using System;

[GlobalClass]
public partial class TaskRequirement : Resource
{
    [Signal] public delegate void ConditionFulfilledEventHandler();

    public bool Fulfilled { get; private set; } = false;

    private bool isActive = false;

    public void Activate()
    {
        if (isActive)
        {
            return;
        }

        Fulfilled = false;
        isActive = true;
        StartListening();
    }

    public void Deactivate()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;
        StopListening();
    }

    protected void Complete()
    {
        if (!isActive)
        {
            return;
        }

        Fulfilled = true;
        Deactivate();
        EmitSignal(SignalName.ConditionFulfilled);
    }

    protected virtual void StartListening() { }

    protected virtual void StopListening() { }
}
