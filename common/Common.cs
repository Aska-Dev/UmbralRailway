using Godot;
using System;

public enum Directions
{
    North,
    East,
    West,
    South
}

public static class Common
{
    public static Directions GetOppositeDirection(Directions direction)
        {
            return direction switch
            {
                Directions.North => Directions.South,
                Directions.East => Directions.West,
                Directions.South => Directions.North,
                Directions.West => Directions.East,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Invalid direction: {direction}")
            };
    }

    public static string GetResourceNameFromPath(string resourcePath)
    {
        var name = System.IO.Path.GetFileNameWithoutExtension(resourcePath);

        GD.Print($"Extracted resource name '{name}' from path '{resourcePath}'");
        return name;
    }
}
