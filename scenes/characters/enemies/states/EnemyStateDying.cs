using Godot;
using System;

public partial class EnemyStateDying : EnemyState
{
    private float durationRagdollSimulation = 3f;

    public EnemyStateDying(Enemy enemy, EnemyStateData data) : base(enemy, data)
    {
    }

    public override void _EnterTree()
	{
		_enemy.equipment.ThrowWeapon(true);
		_enemy.collision.Disabled = true;
        _enemy.skeletonSimulator.Active = true;
        _enemy.skeletonSimulator.PhysicalBonesStartSimulation();
        _enemy.torso.ApplyImpulse(_data.Impulse);
        var timer = GetTree().CreateTimer(durationRagdollSimulation);
        timer.Timeout += FreezeRagdoll;
	}

    private void FreezeRagdoll()
    {
        TransitionState(Enemy.State.DEAD);
    }

}
