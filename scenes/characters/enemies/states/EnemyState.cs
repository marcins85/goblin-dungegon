using Godot;
using System;

public partial class EnemyState : Node
{
	[Signal]
	public delegate void TransitionRequestedEventHandler(int newState);

	private Enemy _enemy;

	public EnemyState(Enemy enemy)
	{
		_enemy = enemy;
	}

	public void TransitionState(Enemy.State newState)
	{
		EmitSignal(SignalName.TransitionRequested, (int)newState);
	}
}
