class_name Player
extends CharacterBody3D


@export var acceleration: float
@export var mouse_sensitivity: float
@export var walk_speed: float
@export var run_speed: float
@export var jump_force: float
@export var gravity: float
@onready var camera: Camera3D = %Camera3D

const MAX_CAMERA_LOOK_UP : float = deg_to_rad(70)
const MAX_CAMERA_LOOK_DOWN : float = deg_to_rad(-70)

var input_dir := Vector2.ZERO
var movement_speed: float


func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)


func _process(_delta: float) -> void:
	input_dir = Input.get_vector("strafe_left", "strafe_right", "backward", "forward")
	movement_speed = run_speed if Input.is_action_pressed("run") else walk_speed


func _physics_process(delta: float) -> void:
	check_jump_input()
	process_gravity()
	
	var input_3d_space := Vector3(input_dir.x, 0, -input_dir.y)
	var desired_velocity := transform.basis * input_3d_space * movement_speed
	
	if input_3d_space == Vector3.ZERO:
		velocity.x = move_toward(velocity.x, 0, delta * acceleration)
		velocity.z = move_toward(velocity.z, 0, delta * acceleration)
	else:
		velocity.x = move_toward(velocity.x, desired_velocity.x, delta * acceleration)
		velocity.z = move_toward(velocity.z, desired_velocity.z, delta * acceleration)
	
	move_and_slide()


func _input(event: InputEvent) -> void:
	if (event is InputEventMouseMotion):
		rotate_y(-event.relative.x * mouse_sensitivity)
		camera.rotate_x(-event.relative.y * mouse_sensitivity)
		camera.rotation.x = clampf(camera.rotation.x, MAX_CAMERA_LOOK_DOWN, MAX_CAMERA_LOOK_UP)


func check_jump_input() -> void:
	if is_on_floor() and Input.is_action_just_pressed("jump"):
		velocity.y += jump_force


func process_gravity() -> void:
	if not is_on_floor():
		velocity.y -= gravity
