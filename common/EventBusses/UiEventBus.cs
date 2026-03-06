using Godot;
using System;

public partial class UiEventBus : Node
{
    public static UiEventBus Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }

    // Interaction Label Events
    [Signal]
    public delegate void ChangeInteractionLabelTextEventHandler(string label);
    public void ShowInteractionText(string text) => EmitSignal(SignalName.ChangeInteractionLabelText, text);
    public void ClearInteractionText() => EmitSignal(SignalName.ChangeInteractionLabelText, string.Empty);

    // Hint Label Events
    [Signal]
    public delegate void ChangeHintLabelTextEventHandler(string label);
    public void ShowHintText(string text) => EmitSignal(SignalName.ChangeHintLabelText, text);
    public void ClearHintText() => EmitSignal(SignalName.ChangeHintLabelText, string.Empty);

    // Computer Screen Events
    [Signal]
    public delegate void ComputerScreenToggledEventHandler(bool isOpen);
    public void ToggleComputerScreen(bool isOpen) => EmitSignal(SignalName.ComputerScreenToggled, isOpen);

    // Inventory Events
    [Signal]
    public delegate void InventoryToggledEventHandler(bool isOpen);
    public void ToggleInventory(bool isOpen) => EmitSignal(SignalName.InventoryToggled, isOpen);

    [Signal]
    public delegate void InventoryUpdatedItemDetailsEventHandler(string itemName, string itemDesc);
    public void InventoryUpdateItemDetails(string itemName, string itemDesc) => EmitSignal(SignalName.InventoryUpdatedItemDetails, itemName, itemDesc);

    // Reading Note Events
    [Signal]
    public delegate void NoteReadingToggledEventHandler(bool isOpen);
    public void ToggleNoteReading(bool isOpen) => EmitSignal(SignalName.NoteReadingToggled, isOpen);

    // Manuals Events
    [Signal]
    public delegate void ManualsToggledEventHandler(bool isOpen);
    public void ToggleManuals(bool isOpen) => EmitSignal(SignalName.ManualsToggled, isOpen);

    // Map Events
    [Signal]
    public delegate void MapToggledEventHandler(bool isOpen);
    public void ToggleMap(bool isOpen) => EmitSignal(SignalName.MapToggled, isOpen);

    // Dialog Events
    [Signal]
    public delegate void DialogTextChangedEventHandler(string text);
    public void SetDialogText(string text) => EmitSignal(SignalName.DialogTextChanged, text);
    
    [Signal]
    public delegate void DialogNextLineShownEventHandler();
    public void ShowNextDialogLine() => EmitSignal(SignalName.DialogNextLineShown);
    
    [Signal]
    public delegate void DialogFinishedEventHandler();
    public void FinishDialog() => EmitSignal(SignalName.DialogFinished);
}

