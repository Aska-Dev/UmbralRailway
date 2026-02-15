using Godot;
using System;

public partial class Note : StaticBody3D, IEntity
{
    public ComponentList Components { get; set; } = null!;

    [Export] public string Title { get; set; } = string.Empty;
    [Export(PropertyHint.MultilineText)] public string Content { get; set; } = string.Empty;

    public override void _Ready()
    {
        Components = new ComponentList(this);
    }

    private void OnInteract()
    {
        UiEventBus.Instance.ToggleNoteReading(true);

        var note = GetTree().GetFirstNodeInGroup("note") as ReadingNote;
        if (note is not null)
        {
            note.SetContent(Title, Content);
        }
    }
}
