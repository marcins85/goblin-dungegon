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
	private readonly float MAX_CAMERA_LOOK_UP = Mathf.DegToRad(70);
	private readonly float MAX_CAMERA_LOOK_DOWN = Mathf.DegToRad(-70);
	private Vector2 _inputDir = Vector2.Zero;
	private float _movementSpeed;


	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{
		_inputDir = Input.GetVector("starfe_left", "strafe_right", "backward", "forward");
		_movementSpeed = Input.IsActionPressed("run") ? _runSpeed : _walkSpeed;
	}

    public override void _PhysicsProcess(double delta)
    {
        CheckJumpInput();
		ProcessGravity();

		// var input3DSpace = new Vector3()
    }

    private void ProcessGravity()
    {
        throw new NotImplementedException();
    }


    private void CheckJumpInput()
    {
        throw new NotImplementedException();
    }
}
