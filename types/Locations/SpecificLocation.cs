using Godot;
using System;
using System.IO;

[GlobalClass]
public partial class SpecificLocation : Location
{
    private const int LocationLeavingDistance = 100;

    public override void MoveForward(Directions facingDirection)
    {
        base.MoveForward(facingDirection);

        if(Position >= LocationLeavingDistance)
        {
            GD.Print("Player has reached the end of the location and is leaving.");
            var approachingNeighbour = GetApproachingNeighbour(TrainMotion.Forward, facingDirection)!;

            var unknownLocation = new Unknown
            {
                Neighbours =
                [
                    new Neighbour
                    {
                        LocationId = approachingNeighbour.LocationId,
                        Distance = approachingNeighbour.Distance,
                        Direction = facingDirection,
                    },
                    new Neighbour
                    {
                        LocationId = LocationId,
                        Direction = Common.GetOppositeDirection(facingDirection),
                    }
                ]
            };

            TrainEventBus.Instance.TravelToLocation(unknownLocation);
        }
    }

    public override void MoveBackward(Directions facingDirection)
    {
        base.MoveBackward(facingDirection);
        if(Position <= -LocationLeavingDistance)
        {
            GD.Print("Player has reached the start of the location and is leaving.");
            var approachingNeighbour = GetApproachingNeighbour(TrainMotion.Backward, facingDirection)!;

            var unknownLocation = new Unknown
            {
                Neighbours =
                [
                    new Neighbour
                    {
                        LocationId = approachingNeighbour.LocationId,   
                        Distance = approachingNeighbour.Distance,
                        Direction = Common.GetOppositeDirection(facingDirection),
                    },
                    new Neighbour
                    {
                        LocationId = LocationId,
                        Direction = facingDirection,
                    }
                ]
            };

            TrainEventBus.Instance.TravelToLocation(unknownLocation);
        }
    }
}
