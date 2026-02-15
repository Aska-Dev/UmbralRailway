using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;



public partial class PlayerGui : Control
{
    private Label interactionMessageLabel = null!;
    private Label hintLabel = null!;

    private UiSceneData computerScreenData = null!;
    private UiSceneData inventoryData = null!;
    private UiSceneData readingNoteData = null!;
    private UiSceneData manualsData = null!;

    public override void _Ready()
	{
        var computerScreenScene = GD.Load<PackedScene>("res://ui/gui/ComputerScreen/ComputerScreen.tscn");
        var inventoryScene = GD.Load<PackedScene>("res://ui/gui/Inventory/Inventory.tscn");
        var readingNoteScene = GD.Load<PackedScene>("res://ui/gui/ReadingNote/ReadingNote.tscn");
        var manualsScene = GD.Load<PackedScene>("res://ui/gui/Manuals/Manuals.tscn");

        computerScreenData = new UiSceneData(computerScreenScene);
        inventoryData = new UiSceneData(inventoryScene);
        readingNoteData = new UiSceneData(readingNoteScene);
        manualsData = new UiSceneData(manualsScene);

        interactionMessageLabel = GetNode<Label>("InteractionMessageLabel");
        hintLabel = GetNode<Label>("HintLabel");

        UiEventBus.Instance.ChangeHintLabelText += UpdateHintLabel;
        UiEventBus.Instance.ChangeInteractionLabelText += UpdateInteractionMessageLabel;

        UiEventBus.Instance.ComputerScreenToggled += (bool isOpen) => computerScreenData.Toggle(isOpen, this);
        UiEventBus.Instance.NoteReadingToggled += (bool isOpen) => readingNoteData.Toggle(isOpen, this);
        UiEventBus.Instance.InventoryToggled += (bool isOpen) => inventoryData.Toggle(isOpen, this);
        UiEventBus.Instance.ManualsToggled += (bool isOpen) => manualsData.Toggle(isOpen, this);
    }

    private void UpdateInteractionMessageLabel(string text)
    {
        interactionMessageLabel.Text = text;
    }

    private void UpdateHintLabel(string text)
    {
        hintLabel.Text = text;
    }
}

public class UiSceneData
{
    public PackedScene Scene { get; set; }
    public bool IsOpen { get; set; } = false;

    private string instanceName = string.Empty;

    public UiSceneData(PackedScene scene)
    {
        Scene = scene;
    }

    public void Toggle(bool isOpen, Control parent)
    {
        if (isOpen == IsOpen)
        {
            return;
        }

        IsOpen = isOpen;
        if(isOpen)
        {
            UiEventBus.Instance.ClearHintText();

            var instance = Scene.Instantiate();
            instanceName = instance.Name;
            parent.AddChild(instance);

        }
        else
        {
            var node = parent.GetNodeOrNull(instanceName);
            if(node != null)
            {
                node.QueueFree();
            }
        }
    }
}
