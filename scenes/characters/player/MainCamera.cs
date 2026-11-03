using Godot;
using System;
using System.Collections.Generic;

public partial class MainCamera : Camera3D
{
	public readonly Dictionary<GameEvents.ImpactIntensity, int> _durationMap = new()
	{
		{ GameEvents.ImpactIntensity.LOW, 140 },
		{ GameEvents.ImpactIntensity.MEDIUM, 200 },
		{ GameEvents.ImpactIntensity.HIGH, 260 },
	};

	public readonly Dictionary<GameEvents.ImpactIntensity, float> _intensityMap = new()
	{
		{ GameEvents.ImpactIntensity.LOW, 0.1f },
		{ GameEvents.ImpactIntensity.MEDIUM, 0.15f },
		{ GameEvents.ImpactIntensity.HIGH, 0.2f },
	};

	public GameEvents.ImpactIntensity CurrentIntensity {get; set;}
	private float _timeStartShake = Time.GetTicksMsec();
	private bool _isShaking = false;
	private Transform3D _initialTransform;

	public override void _Ready()
	{
		GameEvents.Instance.ImpactFelt += OnImpactFelt;
	}

    private void OnImpactFelt(int intensity)
    {
        if (!_isShaking)
		{
			_isShaking = true;
			_timeStartShake = Time.GetTicksMsec();
			_initialTransform = Transform;
			CurrentIntensity = (GameEvents.ImpactIntensity)intensity;
		}
    }

    public override void _Process(double delta)
	{
		if (_isShaking)
		{
			var targetTransform = Transform;
			float durationSinceShake = Time.GetTicksMsec() - _timeStartShake;
			
			if (durationSinceShake < _durationMap[CurrentIntensity])
			{
				var shakeIntensity = _intensityMap[CurrentIntensity];
				var offset = new Vector3((float)GD.RandRange(-shakeIntensity, shakeIntensity), (float)GD.RandRange(-shakeIntensity, shakeIntensity), 0f);
				targetTransform.Origin = _initialTransform.Origin + offset;
			}
			else
			{
				_isShaking = false;
				targetTransform.Origin = _initialTransform.Origin;
			}

			Transform = targetTransform;
		}
	}
}
