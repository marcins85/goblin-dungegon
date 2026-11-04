using Godot;
using System;

public partial class FxHelper : Node
{
	public static readonly PackedScene BloodSportPrefab =
        GD.Load<PackedScene>("res://fx/blood_sport.tscn");

	public static FxHelper Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

	public void CreateBloodFx(Transform3D particleTransform, bool sparkShown = true)
	{
		var blood = BloodSportPrefab.Instantiate<BloodSport>();
		blood.isSparkShown = sparkShown;
		GameState.Instance.CurrentLevel.AddChild(blood);
		blood.GlobalTransform = particleTransform;
	}
}
