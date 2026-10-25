using Godot;
using System;

public partial class EnemyStateHurt : EnemyState
{
    public EnemyStateHurt(Enemy enemy, EnemyStateData data = null) : base(enemy, data)
    {
    }

    public override void _EnterTree()
	{
		_enemy.animationPlayer.Play("hurt");
        _enemy.animationPlayer.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(StringName animName)
    {
        TransitionState(Enemy.State.MOVING);
    }

}
