using DungeonLetter.Common;
using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody3D, IEntity
{
    [Export] public float WalkingSpeed { get; set; } = 5f;

    public ComponentList Components { get; set; } = null!;
    public List<Item> Items { get; set; } = [];

    public bool InputEnabled { get; set; } = true;

    public Item? CurrentItem => playerHand.Item;

    private PlayerHand playerHand = null!;
    private Node3D pivot = null!;
    private Camera3D camera = null!;

    public override void _Ready()
    {
        Components = new ComponentList(this);
        AddToGroup("player");

        pivot = GetNode<Node3D>("Pivot");
        playerHand = pivot.GetNode<PlayerHand>("PlayerHand");
        camera = pivot.GetNode<Camera3D>("Camera3D");

        // Connect to PlayerEventBus to listen for input status changes
        PlayerEventBus.Instance.ChangePlayerInputStatus += OnChangePlayerInputStatus;
        PlayerEventBus.Instance.ItemPickedUp += OnItemPickedUp;
        PlayerEventBus.Instance.ItemRemoved += OnItemRemoved;
        PlayerEventBus.Instance.PickedUpDiskFromComputer += PickUpDiskFromComputer;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed(Inputs.Debug))
        {
            TrainEventBus.Instance.AssignNewTask(TaskRegistry.Instance.Get(1));
        }

        if(!InputEnabled)
        {
            return;
        }

        // Handle camera control
        if (@event is InputEventMouseMotion mouseMotion)
        {
            pivot.RotateY(-mouseMotion.Relative.X * 0.002f);
            camera.RotateX(-mouseMotion.Relative.Y * 0.002f);

            var clampedRotation = camera.Rotation;
            clampedRotation.X = Mathf.Clamp(camera.Rotation.X, -(Mathf.Pi / 2), Mathf.Pi / 2);
            camera.Rotation = clampedRotation;
        }

        if(@event.IsActionPressed(Inputs.Inventory))
        {
            UiEventBus.Instance.ToggleInventory(true);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!InputEnabled)
        {
            return;
        }

        HandlePlayerMovement(delta);
    }

    private void HandlePlayerMovement(double delta)
    {
        var velocity = Velocity;
        var speed = WalkingSpeed;

        // Handle floor movement
        var inputDirection = Input.GetVector(Inputs.MoveLeft, Inputs.MoveRight, Inputs.MoveForward, Inputs.MoveBack);
        var direction = (pivot.GlobalTransform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnChangePlayerInputStatus(bool isEnabled)
    {
        InputEnabled = isEnabled;
        Components.Get<InteractionRayComponent>()!.SetActive(isEnabled);
    }

    private void OnItemPickedUp(Item item)
    {
        Items.Add(item);

        PlayerEventBus.Instance.EquipItem(item);

        UiEventBus.Instance.ShowHintText($"{item.Name} was added to the backpack");
        GetTree().CreateTimer(2f).Timeout += () =>
        {
            UiEventBus.Instance.ShowHintText(string.Empty);
        };
    }

    private void OnItemRemoved(Item item)
    {
        if(CurrentItem == item)
        {
            PlayerEventBus.Instance.EquipItem(null);
        }

        Items.Remove(item);
        UiEventBus.Instance.ShowHintText($"{item.Name} was removed from the backpack");
        GetTree().CreateTimer(2f).Timeout += () =>
        {
            UiEventBus.Instance.ShowHintText(string.Empty);
        };
    }

    private void PickUpDiskFromComputer()
    {
        var train = GetTree().GetFirstNodeInGroup("train") as Train;
        if(train is not null && train.ComputerData.InsertedFloppyDisc is not null)
        {
            var disk = train.ComputerData.InsertedFloppyDisc;
            TrainEventBus.Instance.UpdateTrainFloppyDisc(null);
            PlayerEventBus.Instance.PickUpItem(disk);
        }
    }
}
