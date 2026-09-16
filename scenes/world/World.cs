using Godot;
using Godot.Collections;
using System;

public partial class World : Node3D
{
    private static readonly Array<PackedScene> Levels =
        [GD.Load<PackedScene>("res://scenes/levels/level_01_welcome.tscn")];

    private int _currentLevelIndex = 0;
    private BaseLevel _currentLoadedLevel = null;

    public override void _Ready()
    {
        LoadLevel(_currentLevelIndex);
    }

    private void LoadLevel(int index)
    {
        if (_currentLoadedLevel != null)
        {
            _currentLoadedLevel.QueueFree();
        }
        if (Levels.Count > index)
        {
            _currentLoadedLevel = Levels[index].Instantiate<BaseLevel>();
            GameState.Instance.RegisterLevel(_currentLoadedLevel);
            AddChild(_currentLoadedLevel);
        }
    }
}
