using DungeonLetter.Common;
using Godot;
using System;

public partial class ItemSlot : Control
{
    public required Item Item { get; set; }

    private Panel slotBorder = null!;
    private TextureRect icon = null!;

    private bool _mouseHovering = false;

    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");
        slotBorder = GetNode<Panel>("SlotBorder");
    }

    public override void _Input(InputEvent @event)
    {
        if (_mouseHovering && @event.IsActionPressed(Inputs.Leftclick))
        {
            PlayerEventBus.Instance.EquipItem(Item);
        }
    }

    public void SetItem(Item item)
    {
        Item = item;
        icon.Texture = item.Icon;
    }

    private void OnMouseEntered()
	{
        _mouseHovering = true;

        slotBorder.Visible = true;
        UiEventBus.Instance.InventoryUpdateItemDetails(Item.Name, Item.Description);
    }

	private void OnMouseExited()
	{
        _mouseHovering = false;

        slotBorder.Visible = false;
        UiEventBus.Instance.InventoryUpdateItemDetails(string.Empty, string.Empty);
    }
}
