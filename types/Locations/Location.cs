using Godot;
using System;
using System.Linq;

[GlobalClass]
public partial class Location : Resource
{
    [Export]
    public StringName LocationId { get; set; } = "";

    [Export]
    public Neighbour[] Neighbours { get; set; } = [];

    public int Position { get; set; } = 0;

    public virtual void MoveForward(Directions facingDirection)
    {
        if(GetApproachingNeighbour(TrainMotion.Forward, facingDirection) is null)
        {
            TrainEventBus.Instance.ChangeTrainMotion(TrainMotion.Stop);
            return;
        }

        Position++;
        GD.Print($"Moved forward to position {Position} in location {this}");
    }

    public virtual void MoveBackward(Directions facingDirection)
    {
        if (GetApproachingNeighbour(TrainMotion.Backward, facingDirection) is null)
        {
            TrainEventBus.Instance.ChangeTrainMotion(TrainMotion.Stop);
            return;
        }

        Position--;
        GD.Print($"Moved backward to position {Position} in location {this}");
    }

    protected Neighbour? GetApproachingNeighbour(TrainMotion motion, Directions facingDirection)
    {
        var direction = facingDirection;
        if(motion == TrainMotion.Backward)
        {
            direction = Common.GetOppositeDirection(facingDirection);
        }

        return Neighbours.FirstOrDefault(n => n.Direction == direction);
    }
}
