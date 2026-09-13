using Godot;
using System;

public partial class BaseLevel : Node3D
{
	private static readonly PackedScene PlayerPrefab = 
		GD.Load<PackedScene>("res://scenes/characters/player/player.tscn");

	private Node3D _playerSpawn;

	public override void _Ready()
	{
		_playerSpawn = GetNode<Node3D>("PlayerSpawn");
		var player = PlayerPrefab.Instantiate<Player>();
		player.GlobalTransform = _playerSpawn.GlobalTransform;
		AddChild(player);
	}
}
