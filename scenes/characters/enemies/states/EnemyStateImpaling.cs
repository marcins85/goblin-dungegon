using Godot;
using System;

public partial class EnemyStateImpaling : EnemyState
{
    public EnemyStateImpaling(Enemy enemy, EnemyStateData data) : base(enemy, data)
    {
    }

    public override void _EnterTree()
	{
		var impaledItem = Enemy.EquipedItemPrefab.Instantiate<EquipedItem>();
        if (impaledItem != null)
        {
            impaledItem.WeaponData = _data.thrownItem.WeaponData;
            _enemy.torso.AddChild(impaledItem);
            var itemBasis = new Transform3D(_data.thrownBasis, impaledItem.GlobalPosition);
            impaledItem.GlobalTransform = itemBasis;
            impaledItem.TranslateObjectLocal(impaledItem.WeaponData.impaleLocalTranslation);
            impaledItem.RotateObjectLocal(Vector3.Up, impaledItem.WeaponData.imapleLocalRotation);
            _data.thrownItem.QueueFree();
            _enemy.RegisterDeath(_data.thrownBasis * Vector3.Forward * _enemy.impaleIntensity + Vector3.Up * _enemy.impaleIntensity);
        }
	}
}
