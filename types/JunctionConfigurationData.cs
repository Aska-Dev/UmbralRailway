using Godot;
using System;

[GlobalClass]
public partial class JunctionConfigurationData : Resource
{
    [Export]
    public Neighbour? ConfigurationANeighbour { get; set; } = null!;

    [Export]
    public Neighbour? ConfigurationBNeighbour { get; set; } = null!;
}
