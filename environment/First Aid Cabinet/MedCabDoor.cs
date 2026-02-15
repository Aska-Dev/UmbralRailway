using Godot;
using System;

public partial class MedCabDoor : StaticBody3D, IEntity
{
    public ComponentList Components { get; set; } = null!;

    private bool _isOpen = false;

    public override void _Ready()
    {
        Components = new ComponentList(this);
    }

    public void OnInteract()
    {
        var interactionComponent = Components.Get<InteractionComponent>();
        var animationPlayer = GetParent().GetNode<AnimationPlayer>("AnimationPlayer");
        if (_isOpen)
        {
            animationPlayer.Play("close");
            _isOpen = false;
            interactionComponent.InteractionMessage = "Press [F] to open";
        }
        else
        {
            animationPlayer.Play("open");
            _isOpen = true; 
            interactionComponent.InteractionMessage = "Press [F] to close";
        }
    }
}
