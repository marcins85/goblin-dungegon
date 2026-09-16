using Godot;
using System;

public partial class EquipedItem : Node3D
{
	private static readonly Material ZClipMaterial =
		GD.Load<Material>("res://materials/zclip_material.tres");

	[Export]
	private WeaponData _weaponData;
	public WeaponData WeaponData { get => _weaponData; set => _weaponData = value; }
	// [Export]
	public bool hasZClip;

	public override void _Ready()
	{
		var equipedObject = WeaponData.glbMesh.Instantiate();
		if (equipedObject != null)
		{
			AddChild(equipedObject);
			var meshNode = equipedObject.GetChild<MeshInstance3D>(0);
			if (meshNode != null && hasZClip)
			{
				meshNode.MaterialOverride = (Material)ZClipMaterial.Duplicate();
			}
		}
	}
}
