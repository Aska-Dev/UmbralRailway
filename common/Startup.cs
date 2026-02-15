using Godot;
using System;

public partial class Startup : Node
{
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		GetTree().CreateTimer(10f).Timeout += () =>
		{
			TrainEventBus.Instance.AssignNewTask(TaskRegistry.Instance.Get(1));
		};
    }
}
