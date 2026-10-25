using Godot;
using System;

public partial class EquipmentComponent : Node3D
{
	private static readonly PackedScene EquipedItemPrefab =
		GD.Load<PackedScene>("res://scenes/equipment/equiped_item.tscn");

	private static readonly PackedScene ThrownItemPrefab =
		GD.Load<PackedScene>("res://scenes/equipment/throwed_item.tscn");

	[Export]
	public WeaponData weaponData;
	[Export]
	private Node3D _weaponPlaceholder;
	[Export]
	private Node3D _weaponSpawnPosition;
	[Export]
	private RayCast3D _weaponReachRaycast;
	[Export]
	private bool _hasZClip;

	public override void _Ready()
	{
		if (weaponData != null)
		{
			EquipWeapon(weaponData);
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
		weapon.WeaponData = weaponData;
		weapon.hasZClip = _hasZClip;
		this.weaponData = weaponData;
		_weaponPlaceholder.AddChild(weapon);
		var targetPosition = _weaponReachRaycast.TargetPosition;
		targetPosition.Z = -Mathf.Sqrt(this.weaponData.reach);

		if (pickupTransform != Transform3D.Identity)
		{
			weapon.GlobalTransform = pickupTransform;
			AnimateToHand(weapon);
		}
	}

	public void ThrowWeapon(bool isBeingDropped = false)
	{
		if (HasWeapon())
		{
			var thrownItem = ThrownItemPrefab.Instantiate<ThrowedItem>();
			if (thrownItem is IPickable thrown && thrownItem is Node3D node)
			{
				thrown.WeaponData = weaponData;
				thrownItem.isBeingDropped = isBeingDropped;
				
				var spawnTransform = _weaponPlaceholder.GlobalTransform;
				if (!isBeingDropped)
				{
					spawnTransform = _weaponSpawnPosition.GlobalTransform;
				}
				thrown.GlobalTransform = spawnTransform;

				GameState.Instance.CurrentLevel.AddChild(node);
				weaponData = null;
				_weaponPlaceholder.GetChild(0).QueueFree();
			}
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

	public bool HasWeapon()
	{
		return weaponData != null && _weaponPlaceholder.GetChildCount() > 0;
	}
}
