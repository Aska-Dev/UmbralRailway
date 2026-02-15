using Godot;
using System;

[GlobalClass]
public partial class TrainStation : SpecificLocation
{
    [Export]
    public string Name { get; set; } = "";

    [Export]
    public string Id { get; set; } = "";

    [Export]
    public required StationData Data { get; set; }
} 