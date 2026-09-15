using Godot;
using System;

[GlobalClass]
public partial class WeaponData : Resource
{
	[Export]
	private string _name;
	[Export]
	private int _condition;
	[Export]
	private int _maxCondition;
	[Export]
	private int _damageMin;
	[Export]
	private int _damageMax;
	[Export]
	private float _reach;
	[Export]
	private float _throwRotationSpeed;
	[Export]
	private float _throwMovementSpeed;
	[Export]
	private PackedScene _glbMesh;

	public int GetDamageDealt()
	{
		return GD.RandRange(_damageMin, _damageMax);
	}

	public void DecreaseCondition(int amount)
	{
		_condition = Mathf.Clamp(_condition - amount, 0, _maxCondition);
	}
}
