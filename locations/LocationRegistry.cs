using Godot;
using System;
using System.Collections.Generic;

public partial class LocationRegistry : Node
{
    public static LocationRegistry Instance { get; private set; } = null!;

    private Dictionary<string, Location> cache = new();
    private const string LocationsPath = "res://locations/";

    public override void _Ready()
    {
        Instance = this;
        ScanAndLoad(LocationsPath);
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
                var res = ResourceLoader.Load<Location>(fullPath);
                if (res != null)
                {
                    string id = res.LocationId;

                    if (!cache.TryAdd(id, res))
                    {
                        GD.PushWarning($"Doppelte Location-ID gefunden: {id} in {fullPath}");
                    }
                    else
                    {
                        GD.Print($"Location registriert: {id}");
                    }
                }
            }
            fileName = dir.GetNext();
        }
    }

    public Location? Get(string key) => cache.GetValueOrDefault(key);
}
