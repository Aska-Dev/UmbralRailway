using Godot;
using System;

[GlobalClass]
public partial class TaskBranch : Resource
{
    [Export]
    public Task? NextTask { get; set; }

    [Export]
    public TaskRequirement[] Requirements { get; set; } = Array.Empty<TaskRequirement>();

    private Action<TaskBranch>? completedCallback;
    private bool isActive = false;

    public void Activate(Action<TaskBranch> onCompleted)
    {
        completedCallback = onCompleted;
        isActive = true;

        if (Requirements.Length == 0)
        {
            CompleteBranch();
            return;
        }

        foreach (var requirement in Requirements)
        {
            requirement.ConditionFulfilled += OnRequirementFulfilled;
            requirement.Activate();
        }

        TryComplete();
    }

    public void Deactivate()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;

        foreach (var requirement in Requirements)
        {
            requirement.ConditionFulfilled -= OnRequirementFulfilled;
            requirement.Deactivate();
        }
    }

    private void OnRequirementFulfilled()
    {
        TryComplete();
    }

    private void TryComplete()
    {
        foreach (var requirement in Requirements)
        {
            if (!requirement.Fulfilled)
            {
                return;
            }
        }

        CompleteBranch();
    }

    private void CompleteBranch()
    {
        Deactivate();
        completedCallback?.Invoke(this);
    }
}
