using Godot;
using System;

public partial class EnemyStateMoving : EnemyState
{
	public EnemyStateMoving(Enemy enemy, EnemyStateData data) : base(enemy, data)
	{
	}

    public override void _EnterTree()
    {
        _enemy.animationPlayer.Play("idle");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_enemy.HasRegisteredPlayer())
        {
            if (_enemy.IsPlayerWithinReach() && CanAttack())
            {
                _enemy.timeSinceLastAttack = Time.GetTicksMsec();
                TransitionState(Enemy.State.SLASHING);
            }
        }
    }

    private bool CanAttack()
    {
        return Time.GetTicksMsec() - _enemy.timeSinceLastAttack > _enemy.durationBetweenAttacks;
    }
}
