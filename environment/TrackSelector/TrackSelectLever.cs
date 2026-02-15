using Godot;
using System;

public partial class TrackSelectLever : StaticBody3D, IEntity
{
	[Export]
	public StringName Animation { get; set; } = "";

    public ComponentList Components { get; set; } = null!;

	private AnimationPlayer animationPlayer = null!;
	private AudioStreamPlayer clankSound = null!;

    public override void _Ready()
	{
		animationPlayer = GetParent().GetParent().GetNode<AnimationPlayer>("AnimationPlayer");
		clankSound = GetParent().GetParent().GetNode<AudioStreamPlayer>("ClankSound");

        Components = new ComponentList(this);
    }

	public void OnInteract()
	{
		animationPlayer.Play(Animation);
		clankSound.Play();
    }
}
