using Godot;
using System;

[GlobalClass]
public partial class Neighbour : Resource
{
    [Export]
    public required StringName LocationId { get; set; }

    [Export]
    public int Distance { get; set; } = 10;

    [Export]
    public Directions Direction { get; set; } = Directions.North;

    public Location? GetLocation()
    {
        var location = LocationRegistry.Instance.Get(LocationId);
        
        if (location is null)
        {
            GD.PrintErr($"Location with name '{LocationId}' not found in registry.");
        }

        return location;
    }
}
