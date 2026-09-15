using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	private float _acceleration;
	[Export]
	private float _moueseSensitivity;
	[Export]
	private float _walkSpeed;
	[Export]
	private float _runSpeed;
	[Export]
	private float _jumpForce;
	[Export]
	private float _gravity;
	
	private Camera3D _camera;
	private AnimationPlayer _animationPlayer;
	private readonly float MAX_CAMERA_LOOK_UP = Mathf.DegToRad(70);
	private readonly float MAX_CAMERA_LOOK_DOWN = Mathf.DegToRad(-70);
	private Vector2 _inputDir = Vector2.Zero;
	private float _movementSpeed;
	private RayCast3D _selectRaycast;
	private IHighlightable _currentPickableFocusedItem = null;
	private EquipmentComponent _equipment;

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		_animationPlayer = GetNode<AnimationPlayer>("character/AnimationPlayer");
		_selectRaycast = _camera.GetNode<RayCast3D>("SelectRayCast");
		_equipment = GetNode<EquipmentComponent>("EquipmentComponent");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{
		_inputDir = Input.GetVector("strafe_left", "strafe_right", "backward", "forward");
		_movementSpeed = Input.IsActionPressed("run") ? _runSpeed : _walkSpeed;

		if (Input.IsActionJustPressed("use") && CanPickupObject())
		{
			PickupObject();
		}
	}

    public override void _PhysicsProcess(double delta)
    {
        CheckJumpInput();
		ProcessGravity();

		var input3DSpace = new Vector3(_inputDir.X, 0, -_inputDir.Y);
		var desiredVelocity = Transform.Basis * input3DSpace * _movementSpeed;

		var velocity = Velocity;
		if (input3DSpace == Vector3.Zero)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)delta * _acceleration);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, (float)delta * _acceleration);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, desiredVelocity.X, (float)delta * _acceleration);
			velocity.Z = Mathf.MoveToward(Velocity.Z, desiredVelocity.Z, (float)delta * _acceleration);
		}

		var horizontalVelocity = new Vector3(velocity.X, 0, velocity.Z);
		if (horizontalVelocity.LengthSquared() > 0.1 && IsOnFloor())
		{
			_animationPlayer.Play("run");
		}
		else
		{
			_animationPlayer.Play("idle");
		}

		Velocity = velocity;
		MoveAndSlide();
		CheckForSelection();
    }

    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motion)
		{
			RotateY(-motion.Relative.X * _moueseSensitivity);
			_camera.RotateX(-motion.Relative.Y * _moueseSensitivity);
			var camRotation = _camera.Rotation;
			camRotation.X = Mathf.Clamp(_camera.Rotation.X, MAX_CAMERA_LOOK_DOWN, MAX_CAMERA_LOOK_UP);
			_camera.Rotation = camRotation;
		}	
	}

    private void ProcessGravity()
    {
        if (!IsOnFloor())
		{
			var velocity = Velocity;
			velocity.Y -= _gravity;
			Velocity = velocity;
		}
    }

    private void CheckJumpInput()
    {
        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			var velocity = Velocity;
			velocity.Y += _jumpForce;
			Velocity = velocity;
		}
    }

	private void CheckForSelection()
    {
		IHighlightable targetNode = null;
		if (_selectRaycast.IsColliding())
		{
			var collider = _selectRaycast.GetCollider();
			targetNode = collider as IHighlightable;
		}

		if (targetNode != _currentPickableFocusedItem)
		{
			_currentPickableFocusedItem?.Unhighlight();
			_currentPickableFocusedItem = targetNode;
			_currentPickableFocusedItem?.Highlight();
		}
    }

	private bool CanPickupObject()
	{
		return _currentPickableFocusedItem != null;
	}

	private void PickupObject()
	{
		if (_currentPickableFocusedItem is IPickable pickable)
		{
			_equipment.EquipWeapon(pickable.WeaponData, pickable.TransformItem());
			pickable.Delete();
			_currentPickableFocusedItem = null;
		}
	}
}
