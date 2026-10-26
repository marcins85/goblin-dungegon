using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
	[Export]
	private int _maxLife;
	[Export]
	private int _currentLife;

	public void TakeDamage(int damage)
	{
		_currentLife = Math.Clamp(_currentLife - damage, 0, _maxLife);
	}

	public bool IsDead()
	{
		return _currentLife == 0;
	}
}
