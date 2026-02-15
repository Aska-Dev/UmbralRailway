using Godot;
using System;

[GlobalClass]
public partial class FloppyDisk : Item
{
    [Export(PropertyHint.MultilineText)]
    public string Data { get; set; } = string.Empty;
}
