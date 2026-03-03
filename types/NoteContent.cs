using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

[GlobalClass]
public partial class NoteContent : Resource
{
    [Export]
    public string Title { get; set; } = string.Empty;
    [Export(PropertyHint.MultilineText)]
    public string Content { get; set; } = string.Empty;

    public NoteContent() { }

    public NoteContent(string title, string content)
    {
        Title = title;
        Content = content;
    }
}
