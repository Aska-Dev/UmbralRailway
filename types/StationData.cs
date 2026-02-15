using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

[GlobalClass]
public partial class StationData : Resource
{
    [Export] public int Temperature { get; set; } = 10;

    [Export] public bool TemperatureSensorWorking { get; set; } = true;

    [Export(PropertyHint.Range, "0,3,")] public int WorkingPowerGenerators { get; set; } = 0;

    [Export(PropertyHint.MultilineText)] public string LatestLog { get; set; } = string.Empty;

    [Export] public StationService[] Services { get; set; } = [];
}
