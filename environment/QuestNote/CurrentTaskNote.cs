using Godot;
using System;

public partial class CurrentTaskNote : StaticBody3D, IEntity
{
	public ComponentList Components { get; set; } = null!;

	private NoteContent? contentCache = null;

    public override void _Ready()
	{
		Components = new ComponentList(this);

		TrainEventBus.Instance.MessagePrinted += (NoteContent content) =>
		{
            contentCache = content;
        };
    }

	private void OnInteract()
	{
		UiEventBus.Instance.ToggleNoteReading(true);

        var note = GetTree().GetFirstNodeInGroup("note") as ReadingNote;
		if(note is not null && contentCache is not null)
		{
			note.SetContent(contentCache);
        }
    }
}
