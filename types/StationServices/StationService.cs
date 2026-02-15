using Godot;
using System;

[GlobalClass]
public partial class StationService : Resource
{
    [Export] public bool HasPower { get; set; } = true;
    [Export] public bool IsFunctional { get; set; } = true;

    public virtual string ServiceName { get; } = "StationService";
    public bool IsOperational => HasPower && IsFunctional;

    public virtual void PerformService()
    {
        if(!IsOperational)
        {
            return;
        }
    }
}