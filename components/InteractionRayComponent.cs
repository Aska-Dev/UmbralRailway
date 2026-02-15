using Godot;
using System;

[GlobalClass]
public partial class InteractionRayComponent : Component
{
    [Export]
    public required RayCast3D Ray { get; set; }

    public InteractionComponent? CurrentInteractionComponent { get; private set; }

    private bool _isActive = true;

    public override void _PhysicsProcess(double delta)
    {
        if (Ray.IsColliding() && Ray.GetCollider() is IEntity entity)
        {
            var interactionComponent = entity.Components.Get<InteractionComponent>();
            if (interactionComponent is null)
            {
                return;
            }

            if (CurrentInteractionComponent != interactionComponent)
            {
                if (CurrentInteractionComponent is not null)
                {
                    CurrentInteractionComponent.OnRayHit(false);
                }

                CurrentInteractionComponent = interactionComponent;
                CurrentInteractionComponent.OnRayHit(true);
            }
        }
        else if (CurrentInteractionComponent is not null)
        {
            CurrentInteractionComponent.OnRayHit(false);
            CurrentInteractionComponent = null;
        }
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
        Ray.Enabled = isActive;
    
        if (CurrentInteractionComponent is not null)
        {
            CurrentInteractionComponent.OnRayHit(isActive);
        }
    }
}
