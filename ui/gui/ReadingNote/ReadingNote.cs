using DungeonLetter.Common;
using Godot;

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

            GetViewport().SetInputAsHandled();
        }
    }

    public void SetContent(NoteContent content)
    {         
        this.title.Text = content.Title;
        this.content.Text = content.Content;
    }
}
 