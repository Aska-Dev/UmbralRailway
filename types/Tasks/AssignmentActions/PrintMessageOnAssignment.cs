using Godot;
using System;

[GlobalClass]
public partial class PrintMessageOnAssignment : TaskAssignmentAction
{
    [Export]
    public NoteContent Message { get; set; } = null!;

    public override void Execute()
    {
        TrainEventBus.Instance.PrintMessage(Message);
    }
}
