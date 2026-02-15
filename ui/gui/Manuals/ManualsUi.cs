using DungeonLetter.Common;
using Godot;
using System.Collections.Generic;
using System.IO;

public partial class ManualsUi : Control
{
    private AudioStreamPlayer turnPageSound = null!;

    private Dictionary<string, RichTextLabel> _pages = [];
    private string _currentPage = "Introduction";

    public override void _Ready()
    {
        turnPageSound = GetNode<AudioStreamPlayer>("TurnPageSound");

        PlayerEventBus.Instance.SetPlayerInputEnabled(false);
        Input.MouseMode = Input.MouseModeEnum.Visible;

        var pagesParent = GetNode<Control>("Pages");
        foreach (var page in pagesParent.GetChildren())
        {
            if (page is RichTextLabel richTextLabel)
            {
                _pages.Add(richTextLabel.Name, richTextLabel);
            }
        }

        turnPageSound.Play();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Inputs.Escape) || @event.IsActionPressed(Inputs.Interact))
        {
            UiEventBus.Instance.ToggleManuals(false);
            PlayerEventBus.Instance.SetPlayerInputEnabled(true);
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    public void SelectPage(string pageName)
    {
        if (pageName == _currentPage)
        {
            return;
        }

        turnPageSound.Play();

        _pages[_currentPage].Visible = false;
        _currentPage = pageName;
        _pages[_currentPage].Visible = true;

        // Play flip page sound effect
    }

}
