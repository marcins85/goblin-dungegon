using Godot;
using System;

public partial class GameState : Node
{
	public BaseLevel CurrentLevel { get; private set; }
	public Player CurrentPlayer { get; private set; }

	public static GameState Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void RegisterLevel(BaseLevel level)
	{
		CurrentLevel = level;
	}

	public void RegisterPlayer(Player player)
	{
		CurrentPlayer = player;
	}
}
