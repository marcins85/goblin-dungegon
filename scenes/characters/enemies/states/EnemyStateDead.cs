using Godot;
using System;

public partial class EnemyStateDead : EnemyState
{
    public EnemyStateDead(Enemy enemy, EnemyStateData data) : base(enemy, data)
    {
    }

    public override void _EnterTree()
	{
		foreach (var child in _enemy.skeletonSimulator.GetChildren())
        {
            if (child is PhysicalBone3D bone)
            {
                var boneRID = bone.GetRid();
                PhysicsServer3D.BodySetState(boneRID, PhysicsServer3D.BodyState.Sleeping, true);
            }
        }
	}
}
