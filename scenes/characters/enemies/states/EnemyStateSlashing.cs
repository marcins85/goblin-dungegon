using Godot;
using System;

public partial class EnemyStateSlashing : EnemyState
{
    public EnemyStateSlashing(Enemy enemy, EnemyStateData data = null) : base(enemy, data)
    {
    }


    public override void _EnterTree()
    {
        _enemy.animationPlayer.Play("slash");
		_enemy.animationPlayer.AnimationFinished += OnAnimationFinished;
    }

	public override void _Process(double delta)
	{
	}

    private void OnAnimationFinished(StringName animName)
    {
        TransitionState(Enemy.State.MOVING);
    }
}
