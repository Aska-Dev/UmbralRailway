using DungeonLetter.Common;
using Godot;
using System;

public partial class Inventory : Control
{
    private Player player = null!;

    private Label itemNameLabel = null!;
    private Label itemDescriptionLabel = null!;
    private GridContainer itemGrid = null!;
    private PackedScene itemSlotScene = null!;

    public override void _Ready()
    {
        var playerRef = GetTree().GetFirstNodeInGroup("player") as Player;
        if(playerRef is not null)
        {
            player = playerRef;
        }

        itemNameLabel = GetNode<Label>("ItemName");
        itemDescriptionLabel = GetNode<Label>("ItemDescription");
        itemGrid = GetNode<GridContainer>("ItemGrid");
        itemSlotScene = GD.Load<PackedScene>("res://ui/gui/Inventory/ItemSlot.tscn");

        UiEventBus.Instance.InventoryUpdatedItemDetails += UpdateItemDetails;

        PlayerEventBus.Instance.SetPlayerInputEnabled(false);
        Input.MouseMode = Input.MouseModeEnum.Visible;

        FillInventory();
    }

    public override void _ExitTree()
    {
        UiEventBus.Instance.InventoryUpdatedItemDetails -= UpdateItemDetails;

        PlayerEventBus.Instance.SetPlayerInputEnabled(true);
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed(Inputs.Inventory))
        {
            UiEventBus.Instance.ToggleInventory(false);
            GetViewport().SetInputAsHandled();
        }
    }

    private void UpdateItemDetails(string itemName, string itemDescription)
    {
        itemNameLabel.Text = itemName;
        itemDescriptionLabel.Text = itemDescription;
    }

    private void FillInventory()
    {
        var items = player.Items;
        foreach (var item in items)
        {
            var itemSlotInstance = itemSlotScene.Instantiate<ItemSlot>();
            itemGrid.AddChild(itemSlotInstance);

            itemSlotInstance.SetItem(item);
        }
    }

}
