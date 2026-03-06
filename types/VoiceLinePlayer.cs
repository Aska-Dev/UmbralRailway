using Godot;
using System;
using System.Runtime.CompilerServices;

[GlobalClass]
public partial class VoiceLinePlayer : AudioStreamPlayer
{
	[Export(PropertyHint.MultilineText)]
	public string VoiceLineText { get; set; } = string.Empty;

	private AnimationPlayer animationPlayer = null!;
    private PackedScene radioBeepScene = null!;

    public override void _Ready()
	{
		Finished += OnDialogFinished;

        PlayerEventBus.Instance.SetPlayerInteractable(false);

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        radioBeepScene = GD.Load<PackedScene>("res://VoiceLines/RadioBeepPlayer.tscn");

        PlayRadioBeep(Start);
    }

	public void Start()
	{
        animationPlayer.Play("Play");

        UiEventBus.Instance.SetDialogText(VoiceLineText);
    }

	public void NextLine()
	{
		UiEventBus.Instance.EmitSignal(UiEventBus.SignalName.DialogNextLineShown);
    }

    private void OnDialogFinished()
	{
		UiEventBus.Instance.FinishDialog();

        PlayerEventBus.Instance.SetPlayerInteractable(true);
        PlayRadioBeep(QueueFree);
    }

    private void PlayRadioBeep(Action finishedAction)
    {
        var beepInstance = radioBeepScene.Instantiate<AudioStreamPlayer>();
        GetParent().AddChild(beepInstance);
        
        beepInstance.Finished += () =>
        {
            beepInstance.QueueFree();
            finishedAction();
        };
    }
}
