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

	public void EquipWeapon(WeaponData data)
	{
		EquipWeapon(data, Transform3D.Identity);
	}

    public void EquipWeapon(WeaponData data, Transform3D pickupTransform)
    {
        var weaponData = (WeaponData)data.Duplicate();
		var weapon = EquipedItemPrefab.Instantiate<EquipedItem>();
		weapon.weaponData = weaponData;
		weapon.hasZClip = _hasZClip;
		_weaponPlaceholder.AddChild(weapon);

		if (pickupTransform != Transform3D.Identity)
		{
			weapon.GlobalTransform = pickupTransform;
			AnimateToHand(weapon);
		}
    }

    private void AnimateToHand(EquipedItem weapon)
    {
        var tween = weapon.CreateTween();
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.SetEase(Tween.EaseType.Out);
		tween.Parallel().TweenProperty(weapon, "position", Vector3.Zero, 0.4);
		tween.Parallel().TweenProperty(weapon, "rotation", Vector3.Zero, 0.2);
    }
}
