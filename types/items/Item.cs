using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
    [Export] public required string Name { get; set; }
    [Export] public required string Description { get; set; }
    [Export] public required CompressedTexture2D Icon { get; set; }
    [Export] public PackedScene? Model { get; set; }

    public void PickUp()
    {
        PlayerEventBus.Instance.PickUpItem(this);
    }
}
