using Godot;
using System;

[GlobalClass]
public partial class Task : Resource
{
    [Export]
    public int TaskNumber { get; set; } = 0;

    [Export]
    public string Title { get; set; } = string.Empty;

    [Export]
    public bool Official { get; set; } = true;

    [Export(PropertyHint.MultilineText)]
    public string Description { get; set; } = string.Empty;

    [Export]
    public Task? NextTask { get; set; }

    private bool _isCompleted = false;

    public void Complete()
    {
        if(NextTask is null || _isCompleted)
        {
            return;
        }

        TrainEventBus.Instance.AssignNewTask(NextTask);
        _isCompleted = true;
    }
}
