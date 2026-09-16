using Godot;
using System;

public partial class GameState : Node
{
	private BaseLevel _currentLevel;

	public BaseLevel CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
	public static GameState Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void RegisterLevel(BaseLevel level)
	{
		CurrentLevel = level;
	}
}
