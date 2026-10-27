using Godot;
using System;

public partial class PlayerStateSlashing : PlayerState
{
	private readonly static float TIME_EMIT_DAMAGE = 200f;
	private float _timeStartSlash = Time.GetTicksMsec();
	private bool _hasEmitedDamage = false;

    public PlayerStateSlashing(Player player) : base(player)
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
		_player.animationPlayer.Play("slash");
		_player.animationPlayer.AnimationFinished += OnAnimationFinished;

		
    }

    public override void _Process(double delta)
    {
		float timeElapsed = Time.GetTicksMsec() - _timeStartSlash;
		if (!_hasEmitedDamage && timeElapsed > TIME_EMIT_DAMAGE)
		{
			if (_player.weaponReachRaycast.IsColliding())
			{
				if (_player.weaponReachRaycast.GetCollider() is Enemy enemy)
				{
					_hasEmitedDamage = true;
					int damage = _player.equipment.weaponData.GetDamageDealt();
					enemy.TryReciveHit(_player, damage);
				}
			}
		}
    }


    public override void _PhysicsProcess(double delta)
    {
		_player.ProcessMovement(delta);
    }


    private void OnAnimationFinished(StringName animName)
    {
        TransitionState(Player.State.MOVING);
    }

}
