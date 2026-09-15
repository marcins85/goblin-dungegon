using Godot;
using System;

public partial class PickableItem : Area3D, IHighlightable, IPickable
{
	private static readonly Material HighlighMaterial =
		GD.Load<Material>("res://materials/highlight_material.tres");

	[Export]
	private WeaponData _weaponData;
	private Material _highlightMaterial;
	private MeshInstance3D _meshNode;
	private CollisionShape3D _collision;

    public WeaponData WeaponData { get => _weaponData; set => _weaponData = value; }
	public new Transform3D GlobalTransform { get => base.GlobalTransform; set => base.GlobalTransform = value; }

	public Node3D AsNode()
    {
        return this;
    }

    public override void _Ready()
	{
		_collision = GetNode<CollisionShape3D>("CollisionShape3D");
		_highlightMaterial = (Material)HighlighMaterial.Duplicate();
		if (WeaponData != null)
		{
			Node3D pickableObject = WeaponData.glbMesh.Instantiate<Node3D>();
			if (pickableObject != null)
			{
				AddChild(pickableObject);
				_meshNode = pickableObject.GetChild<MeshInstance3D>(0);
				_collision.Shape = _meshNode.Mesh.CreateConvexShape();
			}
		}
	}

    public void Highlight()
    {
        _meshNode.MaterialOverride = _highlightMaterial;
    }

    public void Unhighlight()
    {
        _meshNode.MaterialOverride = null;
    }

	public void Delete()
	{
		QueueFree();
	}
}
