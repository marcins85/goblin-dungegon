using Godot;
using System;
using System.Collections.Generic;

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
	public AnimationPlayer animationPlayer;
	private readonly float MAX_CAMERA_LOOK_UP = Mathf.DegToRad(70);
	private readonly float MAX_CAMERA_LOOK_DOWN = Mathf.DegToRad(-70);
	private Vector2 _inputDir = Vector2.Zero;
	private float _movementSpeed;
	private RayCast3D _selectRaycast;
	public IHighlightable currentPickableFocusedItem = null;
	public EquipmentComponent equipment;

	public enum State { MOVING, PICKING_UP, THROWING };
	private State _state;
	private PlayerState _stateNode;
	private Dictionary<State, Func<Player, PlayerState>> _stateMap;

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		animationPlayer = GetNode<AnimationPlayer>("character/AnimationPlayer");
		_selectRaycast = _camera.GetNode<RayCast3D>("SelectRayCast");
		equipment = GetNode<EquipmentComponent>("EquipmentComponent");
		Input.MouseMode = Input.MouseModeEnum.Captured;

		_stateMap = new Dictionary<State, Func<Player, PlayerState>>
		{
			{ State.MOVING, Player => new PlayerStateMoving(this) },
			{ State.PICKING_UP, Player => new PlayerStatePickingUp(this) },
			{ State.THROWING, Player => new PlayerStateThrowing(this) }
		};
		SwitchState(State.MOVING);
	}

	public override void _Process(double delta)
	{
		_inputDir = Input.GetVector("strafe_left", "strafe_right", "backward", "forward");
		_movementSpeed = Input.IsActionPressed("run") ? _runSpeed : _walkSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		CheckJumpInput();
		ProcessGravity();
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

	public void ProcessMovement(double delta)
	{
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

		Velocity = velocity;
	}

	public void SwitchState(State newState)
	{
		if (_stateNode != null)
		{
			_stateNode.QueueFree();
		}
		_stateNode = _stateMap[newState](this);
		_stateNode.TransitionRequested += OnTransitionRequested;
		_stateNode.Name = $"State_{newState}";
		_state = newState;
		AddChild(_stateNode);
	}

	private void OnTransitionRequested(int newState)
	{
		SwitchState((State)newState);
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

		if (targetNode != currentPickableFocusedItem)
		{
			currentPickableFocusedItem?.Unhighlight();
			currentPickableFocusedItem = targetNode;
			currentPickableFocusedItem?.Highlight();
		}
	}

	public bool CanPickupObject()
	{
		return currentPickableFocusedItem != null;
	}
}
