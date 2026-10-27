using Godot;
using System;

public partial class EnemyStateData : Resource
{
	public ThrowedItem ThrownItem {get; set;}
	public Basis ThrownBasis {get; set;}
	public Vector3 Impulse {get; set;}
	public int Damage {get; set;}
	public Vector3 ImpactDirection {get; set;}
}
