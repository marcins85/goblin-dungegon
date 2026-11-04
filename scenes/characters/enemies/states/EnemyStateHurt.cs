using Godot;
using System;

public partial class EnemyStateHurt : EnemyState
{
	private static readonly float KNOCKBACK_FORCE = 1.2f;

    public EnemyStateHurt(Enemy enemy, EnemyStateData data = null) : base(enemy, data)
    {
    }

    public override void _EnterTree()
	{
		_enemy.health.TakeDamage(_data.Damage);
		_enemy.pushbackForce += _data.ImpactDirection * KNOCKBACK_FORCE;
		if (_enemy.health.IsDead())
		{
            FxHelper.Instance.CreateBloodFx(_enemy.head.GlobalTransform);
			GameEvents.Instance.EmitSignal(GameEvents.SignalName.ImpactFelt, (int)GameEvents.ImpactIntensity.MEDIUM);
			var data = new EnemyStateData();
			data.Impulse = _data.ImpactDirection * 120.0f + Vector3.Up * 80.0f;
			TransitionState(Enemy.State.DYING, data);
		}
		else
		{
            FxHelper.Instance.CreateBloodFx(_enemy.head.GlobalTransform, false);
			GameEvents.Instance.EmitSignal(GameEvents.SignalName.ImpactFelt, (int)GameEvents.ImpactIntensity.LOW);
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
