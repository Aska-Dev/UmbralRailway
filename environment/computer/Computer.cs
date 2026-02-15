using Godot;
using System;

public partial class Computer : StaticBody3D, IEntity
{
    public ComponentList Components { get; set; } = null!;

    private Player player = null!;
    private AnimationPlayer animationPlayer = null!;

    public override void _Ready()
    {
        var playerRef = GetTree().GetFirstNodeInGroup("player") as Player;
        if(playerRef is not null)
        {
            player = playerRef;
        }

        Components = new ComponentList(this);

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        PlayerEventBus.Instance.UpdateEquippedItem += OnPlayerItemEquipped;
        GlobalEventBus.Instance.ComputerAnimationTriggered += PlayAnimation;
    }

    public void OnPlayerItemEquipped(Item? item)
    {
        if(item is FloppyDisk)
        {
            Components.Get<InteractionComponent>().InteractionMessage = "Press [F] to insert Floppy Disc";
        }
        else
        {
            Components.Get<InteractionComponent>().InteractionMessage = "Press [F] to use the computer";
        }
    }

    public void OnInteract()
    {
        if(player.CurrentItem is FloppyDisk)
        {
            var train = GetTree().GetFirstNodeInGroup("train") as Train;
            if(train is not null)
            {
                if(train.ComputerData.InsertedFloppyDisc is not null)
                {
                    UiEventBus.Instance.ShowHintText("A disc is already inserted. Eject it before inserting another one.");
                    GetTree().CreateTimer(3f).Timeout += () =>
                    {
                        UiEventBus.Instance.ClearHintText();
                    };

                    return;
                }
                else
                {
                    TrainEventBus.Instance.UpdateTrainFloppyDisc(player.CurrentItem as FloppyDisk);
                    PlayerEventBus.Instance.RemoveItem(player.CurrentItem!);
                }
            }

            PlayAnimation("InputDisk");
            PlayerEventBus.Instance.SetPlayerInputEnabled(false);
            GetTree().CreateTimer(1.2f).Timeout += () =>
            {
                UiEventBus.Instance.ToggleComputerScreen(true);
            };
        }
        else
        {
            UiEventBus.Instance.ToggleComputerScreen(true);
            PlayerEventBus.Instance.SetPlayerInputEnabled(false);
        }
    }

    private void PlayAnimation(string animationName)
    {
        if(animationPlayer.HasAnimation(animationName))
        {
            animationPlayer.Play(animationName);
        }
    }
}
