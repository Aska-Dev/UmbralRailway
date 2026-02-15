using Godot;
using System;

public partial class InWorldItem : StaticBody3D, IEntity
{
	[Export] public required Item Item { get; set; }

    public ComponentList Components { get; set; } = null!;

    public override void _Ready()
	{
		Components = new ComponentList(this);
    }

	public void PickUp()
	{
		PlayerEventBus.Instance.PickUpItem(Item);
		QueueFree();
    }
}
