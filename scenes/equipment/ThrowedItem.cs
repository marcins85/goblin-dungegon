using Godot;
using System;

public partial class ThrowedItem : RigidBody3D, IPickable
{
	private static readonly PackedScene PickableItemPrefab =
		GD.Load<PackedScene>("res://scenes/equipment/pickable_item.tscn");

	[Export]
	private WeaponData _weaponData;
	private CollisionShape3D _collision;

    public WeaponData WeaponData { get => _weaponData; set => _weaponData = value; }
	public new Transform3D GlobalTransform { get => base.GlobalTransform; set => base.GlobalTransform = value; }

    public Node3D AsNode()
    {
        return this;
    }


    public void Delete()
    {
        QueueFree();
    }

    public override void _Ready()
	{
		_collision = GetNode<CollisionShape3D>("CollisionShape");
		if (WeaponData != null)
		{
			Node3D thrownObject = WeaponData.glbMesh.Instantiate<Node3D>();
			if (thrownObject != null)
			{
				AddChild(thrownObject);
				var meshNode = thrownObject.GetChild<MeshInstance3D>(0);
				_collision.Shape = meshNode.Mesh.CreateConvexShape();
				BodyEntered += OnBodyEntered;
			}
		}
	}

	private bool sleepingChanged = false;
    private void OnBodyEntered(Node body)
    {
		if (!sleepingChanged)
		{
			SleepingStateChanged += OnSleep;
			sleepingChanged = true;
		}

		// if (!IsConnected("sleeping_state_changed", new Callable(this, nameof(OnSleep))))
		// {
        // 	Connect("sleeping_state_changed", new Callable(this, nameof(OnSleep)));
		// }
    }

	private void OnSleep()
	{
		var pickableItem = PickableItemPrefab.Instantiate<PickableItem>();
		pickableItem.WeaponData = _weaponData;
		pickableItem.GlobalTransform = GlobalTransform;
		GetTree().GetRoot().AddChild(pickableItem);
		QueueFree();
	}
}
