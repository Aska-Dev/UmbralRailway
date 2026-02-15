using Godot;
using System;
using System.Collections.Generic;

public partial class TaskRegistry : Node
{
    public static TaskRegistry Instance { get; private set; } = null!;

    private Dictionary<string, Task> cache = new();
    private const string tasksPath = "res://Tasks/";

    public override void _Ready()
	{
        Instance = this;
        ScanAndLoad(tasksPath);
    }
    private void ScanAndLoad(string path)
    {
        using var dir = DirAccess.Open(path);
        if (dir == null)
        {
            GD.PushError($"Konnte Pfad nicht öffnen: {path}");
            return;
        }

        dir.ListDirBegin();
        string fileName = dir.GetNext();

        while (fileName != "")
        {
            // Pfad kombinieren (Godot-spezifisch sauber gelöst)
            string fullPath = path.PathJoin(fileName);

            if (dir.CurrentIsDir())
            {
                // Ignoriere die Standard-Ordner "." und ".."
                if (fileName != "." && fileName != "..")
                {
                    // REKURSION: Geh eine Ebene tiefer
                    ScanAndLoad(fullPath);
                }
            }
            else if (fileName.EndsWith(".tres"))
            {
                var res = ResourceLoader.Load<Task>(fullPath);
                if (res != null)
                {
                    string id = res.TaskNumber.ToString();

                    if (!cache.TryAdd(id, res))
                    {
                        GD.PushWarning($"Doppelte Task-ID gefunden: {id} in {fullPath}");
                    }
                    else
                    {
                        GD.Print($"Task registriert: {id}");
                    }
                }
            }
            fileName = dir.GetNext();
        }
    }

    public Task? Get(int number) => cache.GetValueOrDefault(number.ToString());
}
