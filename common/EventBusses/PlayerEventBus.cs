using Godot;
using System;

public partial class PlayerEventBus : Node
{
    public static PlayerEventBus Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }

    // Player Events
    [Signal]
    public delegate void ChangePlayerInputStatusEventHandler(bool isEnabled);
    public void SetPlayerInputEnabled(bool isEnabled) => EmitSignal(SignalName.ChangePlayerInputStatus, isEnabled);
    [Signal]
    public delegate void PlayerInteractabilityChangedEventHandler(bool isInteractable);
    public void SetPlayerInteractable(bool isInteractable) => EmitSignal(SignalName.PlayerInteractabilityChanged, isInteractable);


    // Item Events
    [Signal]
    public delegate void ItemPickedUpEventHandler(Item item);
    public void PickUpItem(Item item) => EmitSignal(SignalName.ItemPickedUp, item);

    [Signal]
    public delegate void ItemRemovedEventHandler(Item item);
    public void RemoveItem(Item item) => EmitSignal(SignalName.ItemRemoved, item);

    [Signal]
    public delegate void UpdateEquippedItemEventHandler(Item? item);
    public void EquipItem(Item? item) => EmitSignal(SignalName.UpdateEquippedItem, item);

    // Special Evets
    [Signal]
    public delegate void PickedUpDiskFromComputerEventHandler();
    public void PickUpDiskFromComputer() => EmitSignal(SignalName.PickedUpDiskFromComputer);

    // Camera Evets
    [Signal]
    public delegate void PlayerCameraSetToCurrentEventHandler();
    public void SetPlayerCameraToCurrent() => EmitSignal(SignalName.PlayerCameraSetToCurrent);
}
