using Godot;
using System;

public partial class EnemyStateHurt : EnemyState
{
	private static readonly float KNOCKBACK_FORCE = 0.1f;

    public EnemyStateHurt(Enemy enemy, EnemyStateData data = null) : base(enemy, data)
    {
    }

    public override void _EnterTree()
	{
		_enemy.health.TakeDamage(_data.Damage);
		_enemy.pushbackForce += _data.ImpactDirection * KNOCKBACK_FORCE;
		if (_enemy.health.IsDead())
		{
			var data = new EnemyStateData();
			TransitionState(Enemy.State.DYING, data);
		}
		else
		{
			_enemy.animationPlayer.Play("hurt");
			_enemy.animationPlayer.AnimationFinished += OnAnimationFinished;
		}
    }

    public override void _PhysicsProcess(double delta)
    {
        _enemy.ProcessMovement((float)delta);
    }


    private void OnAnimationFinished(StringName animName)
    {
        TransitionState(Enemy.State.MOVING);
    }

}
