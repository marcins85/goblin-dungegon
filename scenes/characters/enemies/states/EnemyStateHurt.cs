using Godot;
using System;

public partial class EnemyStateHurt : EnemyState
{
    public EnemyStateHurt(Enemy enemy, EnemyStateData data = null) : base(enemy, data)
    {
    }

    public override void _EnterTree()
	{
		_enemy.health.Call("take_damage", _data.Damage);
		GD.Print(_data.Damage);
		if ((bool)_enemy.health.Call("is_dead"))
		{
			var data = new EnemyStateData();
            data.Impulse = 0 * Vector3.Forward * _enemy.impaleIntensity + Vector3.Up * 0;
			TransitionState(Enemy.State.DYING, data);
		}
		else
		{
			_enemy.animationPlayer.Play("hurt");
			_enemy.animationPlayer.AnimationFinished += OnAnimationFinished;
		}
    }

    private void OnAnimationFinished(StringName animName)
    {
        TransitionState(Enemy.State.MOVING);
    }

}
