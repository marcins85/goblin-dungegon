using Godot;
using System;
using System.Collections.Generic;

public partial class HitStopServer : Node
{
	public static HitStopServer Instance { get; private set; }

	private GameEvents.ImpactIntensity _currentIntensity;
	
	private readonly Dictionary<GameEvents.ImpactIntensity, int> _durationMap = new()
	{
		{ GameEvents.ImpactIntensity.LOW, 70 },
		{ GameEvents.ImpactIntensity.MEDIUM, 100 },
		{ GameEvents.ImpactIntensity.HIGH, 130 },
	};

	private bool _isPuased = false;
	private float _timeSinceStartPause = Time.GetTicksMsec();

    public override void _Ready()
    {
		Instance = this;

        ProcessMode = Node.ProcessModeEnum.Always;
		GameEvents.Instance.ImpactFelt += OnImpactFelt;
	}

    private void OnImpactFelt(int intensity)
    {
        GetTree().Paused = true;
		_timeSinceStartPause = Time.GetTicksMsec();
		_isPuased = true;
		_currentIntensity = (GameEvents.ImpactIntensity)intensity;
    }

    public override void _Process(double delta)
    {
        float durationSincePaused = Time.GetTicksMsec() - _timeSinceStartPause;

		if (_isPuased && durationSincePaused > _durationMap[_currentIntensity])
		{
			_isPuased = false;
			GetTree().Paused = false;
		}
    }

}
