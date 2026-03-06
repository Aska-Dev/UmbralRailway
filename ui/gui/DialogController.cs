using Godot;
using System;

public partial class DialogController : Label
{
    private const float DialogTypingSpeed = 30;
    public override void _Ready()
    {
        UiEventBus.Instance.DialogTextChanged += UpdateDialogLabel;
        UiEventBus.Instance.DialogFinished += OnDialogFinished;
    }

    public async void UpdateDialogLabel(string text)
    {
        Visible = true;
        string[] lines = text.Split("//");

        foreach (var line in lines)
        {
            Text = line;
            VisibleCharacters = 0;
            double typingTime = 0;

            while (VisibleCharacters < GetTotalCharacterCount())
            {
                typingTime += GetProcessDeltaTime();
                VisibleCharacters = (int)(DialogTypingSpeed * typingTime);

                await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            }
            
            await ToSignal(UiEventBus.Instance, UiEventBus.SignalName.DialogNextLineShown);
        }
    }

    public void OnDialogFinished()
    {
        Visible = false;
        Text = string.Empty;
        VisibleCharacters = 0;
    }
}
