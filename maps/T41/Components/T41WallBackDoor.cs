using Godot;
using System;
using System.Data;

public partial class T41WallBackDoor : StaticBody3D, IEntity
{
	public ComponentList Components { get; set; } = null!;

	private bool isOpen = false;

	private AnimationPlayer animationPlayer = null!;

    public override void _Ready()
	{
		Components = new ComponentList(this);
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        TrainEventBus.Instance.PowerChanged += (bool hasPower) => Components.Get<InteractionComponent>().IsActive = !hasPower;
    }

	public void OnInteract()
	{
		if (isOpen)
		{
			animationPlayer.Play("close");
			isOpen = false;
		}
		else
		{
			animationPlayer.Play("open");
			isOpen = true;
        }
    }
}
