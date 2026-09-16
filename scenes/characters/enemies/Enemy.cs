using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    private static readonly PackedScene EquipedItemPrefab =
        GD.Load<PackedScene>("res://scenes/equipment/equiped_item.tscn");

    private Node3D _torso;

    public override void _Ready()
    {
        _torso = GetNode<Node3D>("%Physical Bone Torso");
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
        }
    }
}
