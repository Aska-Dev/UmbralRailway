using Godot;
using System;

public partial class Manuals : StaticBody3D, IEntity
{
	public ComponentList Components { get; set; } = null!;

    public override void _Ready()
	{
        Components = new ComponentList(this);
    }

    public void OnInteract()
    {
        UiEventBus.Instance.ToggleManuals(true);
    }
}
