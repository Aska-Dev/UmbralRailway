using DungeonLetter.Common;
using Godot;
using System;
using System.Reflection.Metadata;

public partial class MapUi : Control
{
    public override void _Ready()
    {
        PlayerEventBus.Instance.SetPlayerInputEnabled(false);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Inputs.Escape) || @event.IsActionPressed(Inputs.Interact))
        {
            UiEventBus.Instance.ToggleMap(false);
            PlayerEventBus.Instance.SetPlayerInputEnabled(true);

            GetViewport().SetInputAsHandled();
        }
    }
}
