using Godot;
using System;

public partial class Door : StaticBody3D
{
	private AnimationPlayer animationPlayer;
	private CollisionShape3D collisionShape;

    public override void _Ready()
    {
		animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
		collisionShape = GetNode<CollisionShape3D>("%CollisionShape3D");
    }

	public void Open(Transform3D sourcePosition)
	{
		collisionShape.Disabled = true;
		var doorForward = GlobalBasis.Z;
		var playerForward = sourcePosition.Basis.Z;
		var dotDoor = doorForward.Dot(playerForward);
		if (dotDoor > 0)
		{
			animationPlayer.Play("open-right");
		}
		else
		{
			animationPlayer.Play("open-left");			
		}
	}
}
