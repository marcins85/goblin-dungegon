using Godot;
using System;

public partial class PlayerState : Node
{
	[Signal]
	public delegate void TransitionRequestedEventHandler(int newState);

	protected Player _player;

	public PlayerState(Player player)
	{
		_player = player;
	}

	public void TransitionState(Player.State newState)
	{
		EmitSignal(SignalName.TransitionRequested, (int)newState);
	}
}
