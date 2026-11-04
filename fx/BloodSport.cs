using Godot;
using System;

public partial class BloodSport : Node3D
{
	private GpuParticles3D _blood;
	private GpuParticles3D _sparks;
	public bool isSparkShown = true;

	public override void _Ready()
	{
		_blood = GetNode<GpuParticles3D>("%Blood");
		_sparks = GetNode<GpuParticles3D>("%Sparks");

		_blood.Emitting = true;
		_sparks.Emitting = isSparkShown;

		_blood.Finished += OnBloodFinished;
	}

    private void OnBloodFinished()
    {
        QueueFree();
    }

    public override void _Process(double delta)
	{
	}
}
