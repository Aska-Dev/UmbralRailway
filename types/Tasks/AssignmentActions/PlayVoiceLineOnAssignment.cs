using Godot;
using System;

[GlobalClass]
public partial class PlayVoiceLineOnAssignment : TaskAssignmentAction
{
    [Export]
    public PackedScene VoiceLineScene { get; set; } = null!;

    public override void Execute()
    {
        var audioPlayer = VoiceLineScene.Instantiate<VoiceLinePlayer>();

        var tree = Engine.GetMainLoop() as SceneTree;
        tree?.Root.CallDeferred("add_child", audioPlayer);
    }
}
