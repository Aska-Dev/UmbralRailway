using DungeonLetter.Common;
using Godot;
using System;

[GlobalClass]
public partial class InteractionComponent : Component
{
    [Signal]
    public delegate void OnInteractionEventHandler();

    [ExportGroup("Settings")]
    [Export]
    public required bool IsSingleUse { get; set; } = false;
    [Export]
    public required bool IsActive { get; set; } = true;
    [Export]
    public required string InteractionMessage { get; set; } = "Press [F] to Interact";

    public bool IsLookedAt { get; set; } = false;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Inputs.Interact))
        {
            if (IsActive && IsLookedAt)
            {
                EmitSignal(SignalName.OnInteraction);
                if (IsSingleUse)
                {
                    IsActive = false;
                }

                GetViewport().SetInputAsHandled();
            }
        }
    }

    public void OnRayHit(bool isLookingAt)
    {
        IsLookedAt = isLookingAt;
        if (isLookingAt)
        {
            UiEventBus.Instance.ShowInteractionText(InteractionMessage);
        }
        else
        {
            UiEventBus.Instance.ClearInteractionText();
        }
    }
}
