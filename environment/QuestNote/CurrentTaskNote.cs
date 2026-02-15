using Godot;
using System;

public partial class CurrentTaskNote : StaticBody3D, IEntity
{
	public ComponentList Components { get; set; } = null!;

	private Task? taskCache = null;

    public override void _Ready()
	{
		Components = new ComponentList(this);

		TrainEventBus.Instance.NewTaskAssigned += (Task task) =>
		{ 
			if (task.Official)
			{
				taskCache = task;
            } 
		};
    }

	private void OnInteract()
	{
		UiEventBus.Instance.ToggleNoteReading(true);

        var note = GetTree().GetFirstNodeInGroup("note") as ReadingNote;
		if(note is not null && taskCache is not null)
		{
			note.SetNoteContentFromTask(taskCache);
        }
    }
}
