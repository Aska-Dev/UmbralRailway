using Godot;
using System;

[GlobalClass]
public partial class Task : Resource
{
    [Signal]
    public delegate void TaskCompletedEventHandler(TaskBranch completedBranch);

    [Export]
    public TaskBranch[] Branches { get; set; } = Array.Empty<TaskBranch>();

    [Export]
    public TaskAssignmentAction[] AssignmentActions { get; set; } = Array.Empty<TaskAssignmentAction>();


    private bool hasCompletedBranch = false;

    public void Activate()
    {
        foreach (var branch in Branches)
        {
            branch.Activate(OnBranchCompleted);
        }

        foreach (var action in AssignmentActions)
        {
            action.Execute();
        }
    }

    public void Deactivate()
    {
        foreach (var branch in Branches)
        {
            branch.Deactivate();
        }
    }

    private void OnBranchCompleted(TaskBranch branch)
    {
        if (hasCompletedBranch)
        {
            return;
        }

        hasCompletedBranch = true;
        Deactivate();

        if (branch.NextTask is null)
        {
            GD.PushWarning($"Branch in Task '{ResourceName}' hat keinen Folgetask gesetzt. Die Taskkette endet hier.");
            return;
        }

        EmitSignalTaskCompleted(branch);
    }
}
