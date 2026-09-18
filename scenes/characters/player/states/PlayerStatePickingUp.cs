using Godot;
using System;

public partial class PlayerStatePickingUp : PlayerState
{
	public PlayerStatePickingUp(Player player) : base(player)
	{
	}

	public override void _EnterTree()
	{
		_player.animationPlayer.Play("pickup");
		_player.animationPlayer.AnimationFinished += OnAnimationFinished;

		if (_player.currentPickableFocusedItem is IPickable pickable)
		{
			_player.equipment.EquipWeapon(pickable.WeaponData, pickable.GlobalTransform);
			pickable.Delete();
			_player.currentPickableFocusedItem = null;
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		TransitionState(Player.State.MOVING);
	}
}
