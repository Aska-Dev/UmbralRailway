using DungeonLetter.Common;
using Godot;
using System;

public partial class FuseBoxMinigameUiGameController : Control
{
    [Signal]
    public delegate void FuseBoxMinigamePanelSelectedEventHandler(int turnerIndex);

    private Panel selectionPanel = null!;

    private const int maxX = 200;
    private const int maxY = 200;
    private const int gridsAmount = 3;
    private const int stepSize = 100;

    public override void _Ready()
    {
        selectionPanel = GetNode<Panel>("Panel");
    }

    public void OnInput(InputEvent @event)
    {
        if (@event.IsActionPressed(Inputs.MoveRight))
        {
            MoveSelectionPanel(new Vector2(stepSize, 0));
        }
        else if (@event.IsActionPressed(Inputs.MoveLeft))
        {
            MoveSelectionPanel(new Vector2(-stepSize, 0));
        }
        else if (@event.IsActionPressed(Inputs.MoveBack))
        {
            MoveSelectionPanel(new Vector2(0, stepSize));
        }
        else if (@event.IsActionPressed(Inputs.MoveForward))
        {
            MoveSelectionPanel(new Vector2(0, -stepSize));
        }
        else if (@event.IsActionPressed(Inputs.Space))
        {
            EmitSignal(SignalName.FuseBoxMinigamePanelSelected, GetSelectedTurnerIndex());
        }
    }

    public void HandleMinigameStatusChanged(bool status)
    {
        if (status)
        {
            StartMinigame();
        }
        else
        {
            EndMinigame();
        }
    }

    public void MoveSelectionPanel(Vector2 direction)
    {
        Vector2 newPosition = selectionPanel.Position + direction;

        newPosition.X = Mathf.Clamp(newPosition.X, 0, maxX);
        newPosition.Y = Mathf.Clamp(newPosition.Y, 0, maxY);

        selectionPanel.Position = newPosition;
    }

    private int GetSelectedTurnerIndex()
    {
        int xIndex = (int)(selectionPanel.Position.X / stepSize);
        int yIndex = (int)(selectionPanel.Position.Y / stepSize);

        return yIndex * gridsAmount + xIndex;
    }

    private void StartMinigame()
    {
        selectionPanel.Visible = true;
    }

    private void EndMinigame()
    {
        selectionPanel.Visible = false;
    }
}
