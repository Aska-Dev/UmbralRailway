using Godot;
using System;

[GlobalClass]
public partial class Unknown : Location
{
    private Neighbour? _forwardNeighbour = null;
    private Neighbour? _backwardNeighbour = null; 

    public override void MoveForward(Directions facingDirection)
    {
        base.MoveForward(facingDirection);

        if(_forwardNeighbour is null)
        {
            _forwardNeighbour = GetApproachingNeighbour(TrainMotion.Forward, facingDirection);
        }
        else if(Position >= _forwardNeighbour.Distance)
        {
            TrainEventBus.Instance.TravelToLocation(_forwardNeighbour.GetLocation()!);
        }
    }

    public override void MoveBackward(Directions facingDirection)
    {
        base.MoveBackward(facingDirection);

        if (_backwardNeighbour is null)
        {
            _backwardNeighbour = GetApproachingNeighbour(TrainMotion.Backward, facingDirection);
        }
        else if (Position <= -_backwardNeighbour.Distance)
        {
            TrainEventBus.Instance.TravelToLocation(_backwardNeighbour.GetLocation()!);
        }
    }
}
