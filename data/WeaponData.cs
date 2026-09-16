using Godot;
using System;

[GlobalClass]
public partial class WeaponData : Resource
{
	[Export]
	public string name;
	[Export]
	public int condition;
	[Export]
	public int maxCondition;
	[Export]
	public int damageMin;
	[Export]
	public int damageMax;
	[Export]
	public float reach;
	[Export]
	public float throwRotationSpeed;
	[Export]
	public float throwMovementSpeed;
	[Export]
	public PackedScene glbMesh;

	public int GetDamageDealt()
	{
		return GD.RandRange(damageMin, damageMax);
	}

	public void DecreaseCondition(int amount)
	{
		condition = Mathf.Clamp(condition - amount, 0, maxCondition);
	}
}
