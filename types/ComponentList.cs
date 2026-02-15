using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ComponentList
{
    private List<Component> _components = [];

    public ComponentList(Node node)
    {
        Init(node);
    }

    public T Get<T>(string? name = null) where T : Component
    {
        var t = null as T;

        if (name != null)
        {
            t = _components.FirstOrDefault(c => c is T && c.Name == name) as T;
        }
        else
        {
            t = _components.FirstOrDefault(c => c is T) as T;
        }

        if (t is null)
        {
            GD.PrintErr($"Component of type {typeof(T).Name} with name '{name}' not found.");
            throw new Exception($"Component of type {typeof(T).Name} with name '{name}' not found.");
        }

        return t;
    }

    private void Init(Node node)
    {
        var children = node.GetChildren();

        foreach (var child in children)
        {
            if (child is Component component)
            {
                _components.Add(component);
            }
        }
    }
}
