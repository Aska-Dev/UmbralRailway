using Godot;
using System;

public class TaskManager
{
    public Task AssignedTask { get; private set; } = null!;

	public void Assign(Task task)
	{
		task.Activate();

        AssignedTask = task;
        AssignedTask.TaskCompleted += OnTaskCompleted;
    }

	public void Deassign()
	{
		if (AssignedTask is null)
		{
			return;
		}

		AssignedTask.Deactivate();
		AssignedTask.TaskCompleted -= OnTaskCompleted;

        AssignedTask = null!;
    }

	private void OnTaskCompleted(TaskBranch completedBranch)
	{
		if (completedBranch.NextTask is null)
		{
			GD.PushWarning($"Die Task '{AssignedTask.ResourceName}' hat keinen Folgetask gesetzt. Die Taskkette endet hier.");
			return;
		}

		Assign(completedBranch.NextTask);
    }
}
