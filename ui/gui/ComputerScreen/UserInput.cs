using Godot;
using System;

public partial class UserInput : LineEdit
{
	private AudioStreamPlayer typingSound = null!;
    private AudioStreamPlayer enterSound = null!;

    public override void _Ready()
	{
		typingSound = GetParent().GetNode<AudioStreamPlayer>("TypingSound");
		enterSound = GetParent().GetNode<AudioStreamPlayer>("EnterSound");

		TextChanged += OnTextChanged;
		TextSubmitted += OnTextSubmitted;
	}

	private void OnTextChanged(string newText)
	{
		if (!string.IsNullOrEmpty(newText))
		{
			typingSound.Play();
		}
	}
	private void OnTextSubmitted(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			enterSound.Play();
		}
    }
}
