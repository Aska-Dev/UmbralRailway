using Godot;
using System;

public partial class Generator : Node3D
{
	private AudioStreamPlayer3D hummingSound = null!;
	private AudioStreamPlayer3D startUpSound = null!;
	private AudioStreamPlayer powerDownSound = null!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		hummingSound = GetNode<AudioStreamPlayer3D>("GeneratorHumming");
		startUpSound = GetNode<AudioStreamPlayer3D>("GeneratorStartup");
        powerDownSound = GetNode<AudioStreamPlayer>("GeneratorPowerDown");
        TrainEventBus.Instance.PowerChanged += OnPowerChanged;
    }

	private void OnPowerChanged(bool hasPower)
	{
		if(hasPower)
		{
			startUpSound.Play();
			GetTree().CreateTimer(startUpSound.Stream.GetLength() - 1f).Timeout += () => hummingSound.Play();
        }
		else
		{
			hummingSound.Stop();
			powerDownSound.Play();
        }
    }
}
