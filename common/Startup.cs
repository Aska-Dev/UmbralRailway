using Godot;
using System;

public partial class Startup : Node
{
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		var firstTask = ResourceLoader.Load<ReachingLocationTask>("res://Tasks/0/0-ReachingGehennaStation.tres");

        GetTree().CreateTimer(10f).Timeout += () =>
		{
			TrainEventBus.Instance.AssignNewTask(firstTask);
		};
    }
}
