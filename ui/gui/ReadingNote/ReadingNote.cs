using DungeonLetter.Common;
using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class ReadingNote : Control
{
    private Label title = null!;
    private RichTextLabel content = null!;

    public override void _Ready()
    {
        AddToGroup("note");

        PlayerEventBus.Instance.SetPlayerInputEnabled(false);

        title = GetNode<Label>("Title");
        content = GetNode<RichTextLabel>("Content");

    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed(Inputs.Escape) || @event.IsActionPressed(Inputs.Interact))
        {
            UiEventBus.Instance.ToggleNoteReading(false);
            PlayerEventBus.Instance.SetPlayerInputEnabled(true);
        }
    }

    public void SetNoteContentFromTask(Task task)
    {
        title.Text = $"---### Task {task.TaskNumber} ###---";

        var objective = $"Objective:\n{task.Title}";
        var combinedContent = $"{objective}\n\n{task.Description}";

        content.Text = combinedContent;
    }

    public void SetContent(string title, string content)
    {         
        this.title.Text = title;
        this.content.Text = content;
    }
}
 