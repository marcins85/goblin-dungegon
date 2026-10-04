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


}
