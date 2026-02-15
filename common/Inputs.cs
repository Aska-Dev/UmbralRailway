using System.IO.Pipes;

namespace DungeonLetter.Common;

public static class Inputs
{
    // MOVEMENT
    public const string MoveForward = "move_forward";
    public const string MoveBack = "move_back";
    public const string MoveLeft = "move_left";
    public const string MoveRight = "move_right";

    // UI
    public const string Escape = "escape";
    public const string ScrollUp = "scroll_up";
    public const string ScrollDown = "scroll_down";

    // INTERACTION
    public const string Interact = "interact";
    public const string DropItem = "drop_item";
    public const string Space = "space";
    public const string Inventory = "inventory";

    // MOUSE
    public const string Leftclick = "leftclick";

    // DEBUG
    public const string Debug = "debug";
}