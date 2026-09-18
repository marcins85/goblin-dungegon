using Godot;
using System;

public partial class PlayerStateMoving : PlayerState
{
	public PlayerStateMoving(Player player) : base(player)
	{
	}


	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("use") && _player.CanPickupObject())
		{
			TransitionState(Player.State.PICKING_UP);
		}

		if (Input.IsActionJustPressed("throw") && _player.equipment.HasWeapon())
		{
			TransitionState(Player.State.THROWING);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		_player.ProcessMovement(delta);

		var horizontalVelocity = new Vector3(_player.Velocity.X, 0, _player.Velocity.Z);
		if (horizontalVelocity.LengthSquared() > 0.1 && _player.IsOnFloor())
		{
			_player.animationPlayer.Play("run");
		}
		else
		{
			_player.animationPlayer.Play("idle");
		}
	}
}
