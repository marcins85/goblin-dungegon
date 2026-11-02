using Godot;
using System;

public partial class GameEvents : Node
{
	public enum ImpactIntensity { LOW, MEDIUM, HIGH };
	public static GameEvents Instance { get; private set; }

	[Signal]
	public delegate void ImpactFeltEventHandler(int intensity);

    public override void _Ready()
    {
        Instance = this;
    }

}
