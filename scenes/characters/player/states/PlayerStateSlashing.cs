using Godot;
using System;

public partial class PlayerStateSlashing : PlayerState
{
    public PlayerStateSlashing(Player player) : base(player)
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
		_player.animationPlayer.Play("slash");
		GD.Print("animation slash");
		_player.animationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public override void _PhysicsProcess(double delta)
    {
		_player.ProcessMovement(delta);
    }


    private void OnAnimationFinished(StringName animName)
    {
        TransitionState(Player.State.MOVING);
    }

}
