using DungeonLetter.Common;
using Godot;
using System;

public partial class Bookmark : Control
{
	[Signal] public delegate void ClickedEventHandler(string bookmarkName);

    private bool _mouseHovering = false;

    public override void _Ready()
    {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed(Inputs.Leftclick) && _mouseHovering)
		{
			EmitSignal(SignalName.Clicked, Name);
        }
    }

	public void OnMouseEntered()
	{
		_mouseHovering = true;
    }

	public void OnMouseExited()
	{
		_mouseHovering = false;
    }
}
