using Godot;
using System;

public partial class PlayerStateKicking : PlayerState
{
	private readonly static float GROUND_FRICTION = 10f;

    public PlayerStateKicking(Player player) : base(player)
    {
    }

    public override void _EnterTree()
    {
        _player.animationPlayer.Play("kick");
		_player.animationPlayer.AnimationFinished += OnAnimationFinished;
        if (_player.doorRaycast.IsColliding())
        {
            Door door = _player.doorRaycast.GetCollider() as Door;
            door.Open(_player.GlobalTransform);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
		_player.Velocity = _player.Velocity.MoveToward(Vector3.Zero, (float)delta * GROUND_FRICTION);
    }


    private void OnAnimationFinished(StringName animName)
    {
        TransitionState(Player.State.MOVING);
    }
}
