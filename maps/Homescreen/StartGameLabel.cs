using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class StartGameLabel : Label
{
    [Signal]
    public delegate void HomemenuLabelClickedEventHandler();

    private Line2D line = null!;
    private bool isActive = false;

    public override void _Ready()
    {
        line = GetNode<Line2D>("Line");

        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    public override void _Input(InputEvent @event)
    {
        if (isActive && @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            EmitSignal(SignalName.HomemenuLabelClicked);
        }
    }

    public void OnMouseEntered()
	{
        line.Visible = true;
        isActive = true;
    }

    public void OnMouseExited()
    {
        line.Visible = false;
        isActive = false;
    }
}
