using Godot;
using System;

[GlobalClass]
public partial class ShuntingCrane : StationService
{
    [Export] public Wagon? AvailableWagon { get; set; } = null;

    public override string ServiceName => "SHUNTING_CRANE";

    public override void PerformService()
    {
        base.PerformService();
        if (AvailableWagon is null)
        {
            UiEventBus.Instance.ShowHintText("No wagon is currently available");
            var tree = Engine.GetMainLoop() as SceneTree;
            tree!.CreateTimer(3f).Timeout += () =>
            {
                UiEventBus.Instance.ClearHintText();
            };

            return;
        }
    }
}
