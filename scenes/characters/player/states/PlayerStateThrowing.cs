using Godot;
using System;

public partial class PlayerStateThrowing : PlayerState
{
	public PlayerStateThrowing(Player player) : base(player)
	{
	}

	public override void _EnterTree()
	{
		_player.animationPlayer.Play("throw_weapon");
		_player.animationPlayer.AnimationFinished += OnAnimationFinished;
	}

	private void OnAnimationFinished(StringName animName)
	{
		GD.Print("ThrowWeapon in Throwing State!");
		_player.equipment.ThrowWeapon();
		TransitionState(Player.State.MOVING);
	}
}
