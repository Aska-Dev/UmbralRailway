using Godot;
using System;

[GlobalClass]
public partial class Wagon : Resource
{
    [Export] public int WagonId { get; set; } = 0;
    [Export] public PackedScene? WagonScene { get; set; } = null;
}
