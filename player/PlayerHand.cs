using DungeonLetter.Common;
using Godot;
using System;

public partial class PlayerHand : Node3D
{
    public Item? Item { get; private set; } = null;

    public override void _Ready()
    {
        PlayerEventBus.Instance.UpdateEquippedItem += EquipItem;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed(Inputs.DropItem))
        {
            PlayerEventBus.Instance.EquipItem(null);
        }
    }

    private void EquipItem(Item? item)
    {
        if(item is null)
        {
            DropItem();
            return;
        }

        if (Item is not null)
        {
            DropItem();
        }

        var itemModel = item.Model!.Instantiate<Node3D>();
        AddChild(itemModel);

        Item = item;
    }

    private void DropItem()
    {
        if (Item is null)
        {
            return;
        }

        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }

        Item = null;
    }
}
