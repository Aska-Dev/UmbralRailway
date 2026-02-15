using Godot;
using System;

public interface IEntity
{
    public ComponentList Components { get; set; }
}