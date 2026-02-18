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

    protected void SendMessageToInterface(string message, float hintTextResetTime = 3f, bool resetHint = true)
    {
        TrainEventBus.Instance.UpdateStationServiceNote(message);

        UiEventBus.Instance.ShowHintText("Station Service Message:\n" + message);
        var tree = Engine.GetMainLoop() as SceneTree;

        if(resetHint)
        {
            tree!.CreateTimer(hintTextResetTime).Timeout += () =>
            {
                UiEventBus.Instance.ClearHintText();
            };
        }
    }
}