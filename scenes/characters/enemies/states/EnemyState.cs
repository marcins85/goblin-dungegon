using Godot;
using System;

public partial class EnemyState : Node
{
	[Signal]
	public delegate void TransitionRequestedEventHandler(int newState, EnemyStateData data = null);

	protected Enemy _enemy;
	protected EnemyStateData _data;

	public EnemyState(Enemy enemy, EnemyStateData data = null)
	{
		_enemy = enemy;
		_data = data;
	}

	public void TransitionState(Enemy.State newState, EnemyStateData data = null)
	{
		EmitSignal(SignalName.TransitionRequested, (int)newState, data);
	}
}
