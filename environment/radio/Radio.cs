using Godot;
using System;

public partial class Radio : Node3D
{
    private OmniLight3D light = null!;

    public override void _Ready()
	{
		light = GetNode<OmniLight3D>("Light");

		UiEventBus.Instance.DialogTextChanged += _ => UpdateLight(true);
        UiEventBus.Instance.DialogFinished += () => UpdateLight(false);
    }

	private void UpdateLight(bool lightStatus)
	{
		light.Visible = lightStatus;
    }
}
