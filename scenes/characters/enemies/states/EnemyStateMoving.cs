using Godot;
using System;

public partial class EnemyStateMoving : EnemyState
{
    private static readonly float SPEED_ROTATION = 10f;

	public EnemyStateMoving(Enemy enemy, EnemyStateData data) : base(enemy, data)
	{
	}

    public override void _EnterTree()
    {
        _enemy.animationPlayer.Play("idle");
    }

    public override void _PhysicsProcess(double delta)
    {
        var enemyVelocity = _enemy.Velocity;

        if (_enemy.HasRegisteredPlayer())
        {
            var targetPosition = _enemy.player.GlobalPosition;
            targetPosition.Y = _enemy.GlobalPosition.Y;
            var targetTransform = _enemy.GlobalTransform.LookingAt(targetPosition);
            _enemy.GlobalBasis = _enemy.GlobalBasis.Slerp(targetTransform.Basis, (float)delta * SPEED_ROTATION);
            if (_enemy.IsPlayerWithinReach())
            {
                _enemy.animationPlayer.Play("idle");
                enemyVelocity = new Vector3(0f, enemyVelocity.Y, 0f);
                if (CanAttack())
                {
                    _enemy.timeSinceLastAttack = Time.GetTicksMsec();
                    TransitionState(Enemy.State.SLASHING);
                }
            }
            else
            {
                _enemy.animationPlayer.Play("run");
                _enemy.navAgent.TargetPosition = targetPosition;
                var navAgentNextPath = _enemy.navAgent.GetNextPathPosition();
                var direction = _enemy.GlobalPosition.DirectionTo(navAgentNextPath);
                enemyVelocity = direction * _enemy.speed;
            }
        }

        _enemy.Velocity = enemyVelocity;
        _enemy.ProcessMovement((float)delta);
    }

    private bool CanAttack()
    {
        return Time.GetTicksMsec() - _enemy.timeSinceLastAttack > _enemy.durationBetweenAttacks;
    }
}
