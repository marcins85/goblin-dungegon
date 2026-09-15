using Godot;
using System;

public partial class EquipmentComponent : Node3D
{
	private static readonly PackedScene EquipedItemPrefab =
		GD.Load<PackedScene>("res://scenes/equipment/equiped_item.tscn");

	[Export]
	private WeaponData _weaponData;
	[Export]
	private Node3D _weaponPlaceholder;
	[Export]
	private bool _hasZClip;

	public override void _Ready()
	{
		if (_weaponData != null)
		{
			EquipWeapon(_weaponData);
		}
	}

    private void EquipWeapon(WeaponData data)
    {
        var weaponData = (WeaponData)data.Duplicate();
		var weapon = EquipedItemPrefab.Instantiate<EquipedItem>();
		weapon.weaponData = weaponData;
		weapon.hasZClip = _hasZClip;
		_weaponPlaceholder.AddChild(weapon);
    }

}
