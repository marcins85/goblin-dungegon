using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    private static readonly PackedScene EquipedItemPrefab =
        GD.Load<PackedScene>("res://scenes/equipment/equiped_item.tscn");

    private PhysicalBone3D _torso;
    private PhysicalBoneSimulator3D _skeletonSimulator;
    private CollisionShape3D _collision;

    public override void _Ready()
    {
        _torso = GetNode<PhysicalBone3D>("%Physical Bone Torso");
        _skeletonSimulator = GetNode<PhysicalBoneSimulator3D>("%PhysicalBoneSimulator3D");
        _collision = GetNode<CollisionShape3D>("CollisionShape3D");
    }

    public void Impale(ThrowedItem thrownItem, Basis basis)
    {
        var impaledItem = EquipedItemPrefab.Instantiate<EquipedItem>();
        if (impaledItem != null)
        {
            impaledItem.WeaponData = thrownItem.WeaponData;
            _torso.AddChild(impaledItem);
            impaledItem.GlobalTransform = new Transform3D(basis, impaledItem.GlobalPosition);
            impaledItem.TranslateObjectLocal(impaledItem.WeaponData.impaleLocalTranslation);
            impaledItem.RotateObjectLocal(Vector3.Up, impaledItem.WeaponData.imapleLocalRotation);
            thrownItem.QueueFree();
            RegisterDeath();
        }
    }

    private void RegisterDeath()
    {
        _collision.Disabled = true;
        _skeletonSimulator.Active = true;
        _skeletonSimulator.PhysicalBonesStartSimulation();
    }
}
