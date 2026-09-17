using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    private static readonly PackedScene EquipedItemPrefab =
        GD.Load<PackedScene>("res://scenes/equipment/equiped_item.tscn");

    private PhysicalBone3D _torso;
    private PhysicalBoneSimulator3D _skeletonSimulator;
    private CollisionShape3D _collision;
    private float _impaleIntensity = 100f;
    private float _durationRagdollSimulation = 3f;

    public override void _Ready()
    {
        _torso = GetNode<PhysicalBone3D>("%Physical Bone Torso");
        _skeletonSimulator = GetNode<PhysicalBoneSimulator3D>("%PhysicalBoneSimulator3D");
        _collision = GetNode<CollisionShape3D>("%CollisionShape");
    }

    public void Impale(ThrowedItem thrownItem, Basis basis)
    {
        var impaledItem = EquipedItemPrefab.Instantiate<EquipedItem>();
        if (impaledItem != null)
        {
            impaledItem.WeaponData = thrownItem.WeaponData;
            _torso.AddChild(impaledItem);
            var itemBasis = new Transform3D(basis, impaledItem.GlobalPosition);
            impaledItem.GlobalTransform = itemBasis;
            impaledItem.TranslateObjectLocal(impaledItem.WeaponData.impaleLocalTranslation);
            impaledItem.RotateObjectLocal(Vector3.Up, impaledItem.WeaponData.imapleLocalRotation);
            thrownItem.QueueFree();
            RegisterDeath(basis * Vector3.Forward * _impaleIntensity + Vector3.Up * _impaleIntensity);
        }
    }

    private void RegisterDeath()
    {
        RegisterDeath(Vector3.Zero);
    }

    private void RegisterDeath(Vector3 impulse)
    {
        _collision.Disabled = true;
        _skeletonSimulator.Active = true;
        _skeletonSimulator.PhysicalBonesStartSimulation();
        _torso.ApplyImpulse(impulse);
        var timer = GetTree().CreateTimer(_durationRagdollSimulation);
        timer.Timeout += FreezeRagdoll;
    }

    private void FreezeRagdoll()
    {
        foreach (var child in _skeletonSimulator.GetChildren())
        {
            if (child is PhysicalBone3D bone)
            {
                var boneRID = bone.GetRid();
                PhysicsServer3D.BodySetState(boneRID, PhysicsServer3D.BodyState.Sleeping, true);
            }
        }
    }
}
